using System;

namespace Lesson_8_MyEF.Models
{
    public abstract class User
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
