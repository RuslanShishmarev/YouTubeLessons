using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReadWriteUserToFile
{
    class Program
    {
        static string DBFilePath { get; set; }
        static void Main(string[] args)
        {
            string fileDBName = "users_for_youtube_lessons.txt";
            string fileFolderPath = Path.GetTempPath();
            DBFilePath = fileFolderPath + fileDBName;

            if(File.Exists(DBFilePath) == false)
            {
                var file = File.Create(DBFilePath);
                file.Close();
            }

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
                            var allUsers = ReadAllFromDB();
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

                            User newUser = new User(0, name, surname);
                            SaveToDB(newUser);

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
                                bool result = DeleteFromDB(id);

                                if (result) Console.WriteLine("Успешно");
                                else Console.WriteLine("Ошибка");
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

        static void SaveToDB(User user)
        {
            List<User> allCurrentUsers = ReadAllFromDB();

            int lastId = allCurrentUsers.Count == 0 ? 0 : allCurrentUsers.Last().Id;

            user.SetNewId(lastId + 1);

            allCurrentUsers.Add(user);

            string serializedUsers = JsonConvert.SerializeObject(allCurrentUsers);

            File.WriteAllText(DBFilePath, serializedUsers);
        }

        static void SaveToDB(List<User> users)
        {
            string serializedUsers = JsonConvert.SerializeObject(users);

            File.WriteAllText(DBFilePath, serializedUsers);
        }

        static bool DeleteFromDB(int id)
        {
            List<User> allCurrentUsers = ReadAllFromDB();

            User userForDeletion = allCurrentUsers.FirstOrDefault(u => u.Id == id);

            bool result = false;

            if(userForDeletion != null)
            {
                allCurrentUsers.Remove(userForDeletion);
                SaveToDB(allCurrentUsers);
                result = true;
            }

            return result;
        }

        static List<User> ReadAllFromDB()
        {
            string json = File.ReadAllText(DBFilePath);

            List<User> currentUsers = JsonConvert.DeserializeObject<List<User>>(json);

            return currentUsers ?? new List<User>();
        }
    }

    class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }

        public User(int id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

        public void SetNewId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id} {Name} {Surname}";
        }
    }
}
