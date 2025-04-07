using DataStructures_Lesson.Models;

namespace DataStructures_Lesson_Tests;

public class MyDictionaryTests
{
    [Fact]
    public void Add_And_Get_Works_Correctly()
    {
        var dict = new MyDictionary<Guid, string>();

        for (int i = 0; i < 100; i++)
        {
            var key = Guid.NewGuid();
            dict.Add(key, $"Value {i}");

            Assert.Equal($"Value {i}", dict[key]);
        }
    }

    [Fact]
    public void Indexer_Adds_And_Retrieves_Value()
    {
        var dict = new MyDictionary<string, int>();
        dict["age"] = 30;

        Assert.Equal(30, dict["age"]);
    }

    [Fact]
    public void Remove_Removes_Value()
    {
        var dict = new MyDictionary<string, string>();
        dict.Add("key", "value");

        dict.Remove("key");

        Assert.Null(dict.Get("key"));
    }

    [Fact]
    public void Add_Duplicate_Key()
    {
        var dict = new MyDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(1, "duplicate");

        Assert.Equal("duplicate", dict[1]);
    }

    [Fact]
    public void Enumerator_Returns_All_Elements_WithRebase()
    {
        var dict = new MyDictionary<Guid, int>();
        var keys = new List<Guid>();

        for (int i = 0; i < 100; i++)
        {
            var key = Guid.NewGuid();
            dict.Add(key, i);
            keys.Add(key);
        }

        var result = dict.ToList();

        Assert.Equal(100, result.Count);
        foreach (var key in keys)
        {
            Assert.Contains(result, kvp => kvp.Key == key);
        }
    }

    [Fact]
    public void Enumerator_Returns_All_Elements_WithoutRebase()
    {
        var dict = new MyDictionary<Guid, int>();
        var keys = new List<Guid>();

        for (int i = 0; i < 50; i++)
        {
            var key = Guid.NewGuid();
            dict.Add(key, i);
            keys.Add(key);
        }

        var result = dict.ToList();

        Assert.Equal(50, result.Count);
        foreach (var key in keys)
        {
            Assert.Contains(result, kvp => kvp.Key == key);
        }
    }
}