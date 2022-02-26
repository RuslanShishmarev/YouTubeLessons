using System;

namespace Lesson_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputByUser = Console.ReadLine();
            string inputByUserLower = inputByUser.ToLower();

            bool isStrHello = inputByUserLower.Contains("привет");

            if (isStrHello == true) Console.WriteLine("И тебе привет!");

            Console.WriteLine("Пользователь ввел: " + inputByUser);
        }
    }
}
