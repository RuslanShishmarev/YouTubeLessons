using Lesson_8_MyEF.Models;
using MyDataFramework_Lesson.Models;

namespace Lesson_8_MyEF
{
    public class DataContext : MyDBContext
    {
        public MyDbSet<Worker> Workers { get; set; } = new MyDbSet<Worker>();
        public MyDbSet<Job> Jobs { get; set; } = new MyDbSet<Job>();
    }
}
