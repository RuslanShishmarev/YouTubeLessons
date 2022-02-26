using System;

namespace Lesson_2_CheckBadWords
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] badWords = new string[] { "тупой", "тупая", "тупые", "урод"};

            string textByUser = Console.ReadLine();

            string[] textAsArray = textByUser.Split(' ');

            foreach(string word in textAsArray)
            {
                if(word.Length > 2)
                {
                    foreach(string badW in badWords)
                    {
                        if(word.ToLower().Contains(badW))
                        {
                            Console.WriteLine("Ваше предложение не прошло проверку. Вы нарушили правило сообщества. Подозрительное слово: " + word);
                            return;
                        }
                    }
                }
            }

            Console.WriteLine("Конец программы");
        }
    }
}
