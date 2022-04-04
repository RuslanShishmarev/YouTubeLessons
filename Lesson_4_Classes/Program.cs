using System;
using System.Collections.Generic;

namespace Lesson_4_Classes
{
    class Program
    {
        static List<User> _allUsers = new List<User>();
        static void Main(string[] args)
        {
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("Введите имя пользователя:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите фамилию пользователя:");
                string surname = Console.ReadLine();

                Console.WriteLine("Введите дату рождения в формате гггг/мм/дд");
                string birthdayStr = Console.ReadLine();
                DateTime birthday = GetBirthday(birthdayStr);

                User newUser = new User(name: name, surname: surname, birthday: birthday);
                _allUsers.Add(newUser);

                Console.WriteLine($"Успешно. Пользователь {newUser} зареген");

                Console.WriteLine($"Хотите продолжить? Enter/Delete");
                ConsoleKeyInfo userAnswer = Console.ReadKey();

                if (userAnswer.Key == ConsoleKey.Enter) Console.WriteLine("Поехали\n---------------------");

                if (userAnswer.Key == ConsoleKey.Delete) isWork = false;
            }

            ShowAllUsers();

        }

        static void ShowAllUsers()
        {
            for (int i = 0; i < _allUsers.Count; i++)
            {
                Console.WriteLine($"{i + 1} {_allUsers[i]}"); 
            }
        }

        static DateTime GetBirthday(string userDateInput)
        {
            DateTime date = DateTime.Parse(userDateInput);

            return date;
        }
    }

    class User
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public DateTime Birthday { get; private set; }

        public User(string name, string surname = "Dafault", DateTime? birthday = null)
        {
            Name = name;
            Surname = surname;
            Birthday = birthday ?? DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Name} {Surname} {Birthday.ToShortDateString()}";
        }
    }
}
