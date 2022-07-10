using System;
using System.Collections.Generic;
using System.Linq;

namespace Lesson_4_Classes
{
    class Program
    {
        static List<Worker> _allUsers = new List<Worker>();

        static List<Job> _allJobs = new List<Job>()
        {
            new Job("Программист", 100),
            new Job("PM", 150),
            new Job("Тестер", 100),
        };

        static void Main(string[] args)
        {
            bool isWork = true;

            ShowAllObjs(_allJobs);

            while (isWork)
            {
                Console.WriteLine("Выберите номер вакансии");
                int jobNum = int.Parse(Console.ReadLine());

                Job selectedJob = _allJobs[jobNum - 1];

                Console.WriteLine("Введите имя пользователя:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите фамилию пользователя:");
                string surname = Console.ReadLine();

                Console.WriteLine("Введите дату рождения в формате гггг/мм/дд");
                string birthdayStr = Console.ReadLine();
                DateTime birthday = GetBirthday(birthdayStr);

                Worker newUser = new Worker(selectedJob, name: name, surname: surname, birthday: birthday);
                _allUsers.Add(newUser);

                Console.WriteLine($"Успешно. Пользователь {newUser} зареган");

                Console.WriteLine($"Хотите продолжить? Enter/Delete");
                ConsoleKeyInfo userAnswer = Console.ReadKey();

                if (userAnswer.Key == ConsoleKey.Enter) Console.WriteLine("Поехали\n---------------------");

                if (userAnswer.Key == ConsoleKey.Delete) isWork = false;
            }

            ShowAllObjs(_allUsers);

        }

        static void ShowAllObjs<T>(IList<T> objs)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                Console.WriteLine($"{i + 1} {objs[i]}");
            }
        }

        static DateTime GetBirthday(string userDateInput)
        {
            DateTime date = DateTime.Parse(userDateInput);

            return date;
        }
    }

    abstract class User
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

    class Worker : User
    {
        public Job Job { get; private set; }

        public Worker(Job job, string name, string surname, DateTime? birthday = null) : base(name, surname, birthday)
        {
            Job = job;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {Job}";
        }
    }

    class Job
    {
        public string Title { get; private set; }
        public decimal Salary { get; private set; }

        public Job(string title, decimal salary)
        {
            Title = title;
            Salary = salary;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
