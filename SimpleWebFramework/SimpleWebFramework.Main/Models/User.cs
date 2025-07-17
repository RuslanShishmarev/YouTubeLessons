namespace SimpleWebFramework.Main.Models;

public record User(string Name, int Age)
{
    public static User Create(string name, int age) => new(name, age);
    public override string ToString() => $"{{ \"name\": \"{Name}\", \"age\": {Age} }}";
};
