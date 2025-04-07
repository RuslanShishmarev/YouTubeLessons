namespace DataStructures_Lesson.Models;

public class Person
{
    public int Id { get; set; }

    public Guid TransportId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int SequenceId { get; set; }

    public int Age { get; set; }

    public double Salary { get; set; }

    public bool IsMarred { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
