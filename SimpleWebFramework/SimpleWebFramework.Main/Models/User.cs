namespace SimpleWebFramework.Main.Models;

public record User(int Id, string Name, int Age)
{
    public override string ToString() => $"{{ \"name\": \"{Name}\", \"age\": {Age} }}";
};
