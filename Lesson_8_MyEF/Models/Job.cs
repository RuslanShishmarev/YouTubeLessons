using MyDataFramework_Lesson.Interfaces;

namespace Lesson_8_MyEF.Models
{
    public class Job: IMyDBElement
    {
        public int Id { get; set; }

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
