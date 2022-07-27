using Lesson_8_MyEF;
using Lesson_8_MyEF.Models;

using MyDataFramework_Lesson.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lesson_4_Classes
{
    class Program
    {
        //static List<Worker> _allUsers = new List<Worker>();
        static void Main(string[] args)
        {
            bool isWork = true;
            List<Job> allJobs = new List<Job>()
            {
                new Job("Программист", 100),
                new Job("PM", 150),
                new Job("Тестер", 100),
            
            };
            DataContext dataContext = new DataContext();
            dataContext.SetDBPath("C://Users//Admin//Desktop//Programming//Lesson_8_MyDB");
            if (!dataContext.Jobs.Any()) dataContext.Jobs.AddRange(allJobs);

            ShowAllObjs(dataContext.Jobs);
            Console.WriteLine("----------------------------------");
            ShowAllObjs(dataContext.Workers);

            while (isWork)
            {
                Console.WriteLine("Выберите номер вакансии");
                int jobNum = int.Parse(Console.ReadLine());

                Job selectedJob = dataContext.Jobs.FirstOrDefault(x => x.Id == jobNum);

                Console.WriteLine("Введите имя пользователя:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите фамилию пользователя:");
                string surname = Console.ReadLine();

                Console.WriteLine("Введите дату рождения в формате гггг/мм/дд");
                string birthdayStr = Console.ReadLine();
                DateTime birthday = GetBirthday(birthdayStr);

                Worker newUser = new Worker(selectedJob, name: name, surname: surname, birthday: birthday);
                dataContext.Workers.Add(newUser);

                Console.WriteLine($"Успешно. Пользователь {newUser} зареган");

                Console.WriteLine($"Хотите продолжить? Enter/Delete");
                ConsoleKeyInfo userAnswer = Console.ReadKey();

                if (userAnswer.Key == ConsoleKey.Enter) Console.WriteLine("Поехали\n---------------------");

                if (userAnswer.Key == ConsoleKey.Delete)
                {
                    isWork = false;
                    dataContext.SaveChanges();
                }
            }

            ShowAllObjs(dataContext.Workers);
        }

        static void ShowAllObjs(IEnumerable<IMyDBElement> objs)
        {
            foreach (var item in objs)
            {
                Console.WriteLine($"{item.Id} {item}");
            }
        }

        static DateTime GetBirthday(string userDateInput)
        {
            DateTime date = DateTime.Parse(userDateInput);

            return date;
        }
    }
}
