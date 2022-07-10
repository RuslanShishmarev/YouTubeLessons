using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReadWriteUserToFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileFolderPath = Path.GetTempPath();

            MyAppContext myAppContext = new MyAppContext();
            myAppContext.SetDbPath(Path.Combine(fileFolderPath, "my_test_db/"));

            bool isWork = true;

            string allCommands = "\n0 - Вывести всех \n1 - Добавить нового \n2 - Удалить \n3 - Выход из программы \n------------------";

            while (isWork)
            {
                Console.WriteLine(allCommands);

                string inputCommandStr = Console.ReadLine();

                int inputCommand = 0;

                try
                {
                    inputCommand = int.Parse(inputCommandStr);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Нет такой команды");
                }

                switch (inputCommand)
                {
                    case 0:
                        {
                            var allUsers = myAppContext.Users;
                            if (allUsers.Count == 0) Console.WriteLine("Пока еще нет людей");
                            foreach (var user in allUsers) Console.WriteLine(user);
                            break;
                        }
                    case 1:
                        {
                            Console.WriteLine("Введите имя:");
                            string name = Console.ReadLine();

                            Console.WriteLine("Введите фамилию:");
                            string surname = Console.ReadLine();

                            User newUser = new User(name, surname);
                            Worker newWorker = new Worker(name, surname, "программист");

                            myAppContext.Users.Add(newUser);
                            myAppContext.Workers.Add(newWorker);

                            myAppContext.SaveChanges();

                            Console.WriteLine("Успешно");
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Введите ID:");

                            string idStr = Console.ReadLine();
                            int id = GetIntFromString(idStr);

                            if (id == 0) Console.WriteLine("Нет такого Id");
                            else
                            {
                                try
                                {
                                    User userForDeletion = myAppContext.Users.FirstOrDefault(x => x.Id == id);
                                    myAppContext.Users.Remove(userForDeletion);
                                    myAppContext.SaveChanges();

                                    Console.WriteLine("Успешно");
                                }
                                catch(Exception ex) { Console.WriteLine("Ошибка\n " + ex.Message); }
                            }
                            break;
                        }
                    case 3:
                        {
                            isWork = false;
                            Console.WriteLine("Пока");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Нет такой команды");
                            break;
                        }
                }
            }

        }

        static int GetIntFromString(string inputStr)
        {
            int input = 0;
            try
            {
                input = int.Parse(inputStr);
            }
            catch (FormatException)
            {
            }
            return input;
        }
    }

    class User : IDbElement
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }

        public User(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public override string ToString()
        {
            return $"{Id} {Name} {Surname}";
        }
    }

    class Worker : User
    {
        public string JobTitle { get; private set; }

        public Worker(string name, string surname, string jobTitle) : base(name, surname)
        {
            JobTitle = jobTitle;
        }
    }

    abstract class MyContext
    {
        protected string DB_PATH { get; private set; }

        string fileFormat = ".mydb";

        public virtual void SetDbPath(string db_path)
        {
            DB_PATH = db_path;

            Directory.CreateDirectory(db_path);

            GetValues();
        }

        public void SaveChanges()
        {
            var allDBsAsProps = GetProperties();

            foreach (var set in allDBsAsProps)
            {
                MyDbSet dbSet = set.GetValue(this) as MyDbSet;
                string serialized = JsonConvert.SerializeObject(dbSet);

                string dbPath = Path.Combine(DB_PATH, set.Name + fileFormat);

                File.WriteAllText(dbPath, serialized);

                dbSet.SaveChanges();
            }
        }

        protected IList<PropertyInfo> GetProperties()
        {
            return this.GetType().GetProperties().Where(x => x.PropertyType.BaseType.Name == nameof(MyDbSet)).ToList();
        }

        private void GetValues()
        {
            var allDBsAsProps = GetProperties();
            foreach (var set in allDBsAsProps)
            {
                string dbPath = Path.Combine(DB_PATH, set.Name + fileFormat);
                MyDbSet dbSet = set.GetValue(this) as MyDbSet;

                if (File.Exists(dbPath))
                {
                    string serialized = File.ReadAllText(dbPath);
                    dbSet.Fill(serialized);
                }
            }
        }
    }

    class MyAppContext : MyContext
    {
        public MyDbSet<User> Users { get; set; } = new MyDbSet<User>();
        public MyDbSet<Worker> Workers { get; set; } = new MyDbSet<Worker>();

        public IEnumerable<string> GetAllProps()
        {
            var allDBs = GetProperties();
            return allDBs.Select(x => x.Name);
        }
    }

    interface IDbElement
    {
        int Id { get; set; }
    }

    interface IMyDbSet
    {
        void SaveChanges();
    }

    abstract class MyDbSet : IMyDbSet
    {
        public bool IsChanged { get; protected set; }

        public void SaveChanges()
        {
            IsChanged = false;
        }

        public MyDbSet()
        {
            IsChanged = false;
        }

        public abstract void Fill(string data);
    }
    class MyDbSet<T>: MyDbSet, IEnumerable<T> where T : IDbElement
    {
        private List<T> _innerList = new List<T>();

        public int Count => _innerList.Count;

        public void Add(T item)
        {
            item.Id = (_innerList.LastOrDefault()?.Id ?? 0) + 1;
            _innerList.Add(item);

            IsChanged = true;
        }

        public void Clear()
        {
            _innerList.Clear();
            IsChanged = true;
        }

        public bool Contains(T item)
        {
            return _innerList.Contains(item);
        }
        public bool Remove(T item)
        {
            T itemForRemoved = _innerList.FirstOrDefault(x => x.Id == item.Id);
            IsChanged = true;
            return _innerList.Remove(itemForRemoved);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Fill(string data)
        {
            var deserialized = JsonConvert.DeserializeObject<List<T>>(data);
            _innerList = deserialized;
        }
    }
}
