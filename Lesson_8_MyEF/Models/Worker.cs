using MyDataFramework_Lesson.Interfaces;
using System;

namespace Lesson_8_MyEF.Models
{
    public class Worker : User, IMyDBElement
    {
        public int Id { get; set; }

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
}
