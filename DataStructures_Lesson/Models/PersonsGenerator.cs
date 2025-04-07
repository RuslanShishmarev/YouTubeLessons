namespace DataStructures_Lesson.Models;

public class PersonsGenerator
{
    public List<Person> Persons = new List<Person>();
    private Random random = new Random();

    public PersonsGenerator(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Persons.Add(new Person
            {
                Id = i,
                TransportId = Guid.NewGuid(),

                // Случайная генерация имен
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),

                SequenceId = i,

                // Числа тоже можно генерировать
                Age = Faker.RandomNumber.Next(18, 100),

                Salary = Faker.RandomNumber.Next(10000, 100000),
                IsMarred = Faker.Boolean.Random(),
            });
        }
    }
}
