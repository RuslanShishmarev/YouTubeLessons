// See https://aka.ms/new-console-template for more information
using DataStructures_Lesson.Models;

Console.WriteLine("Hello, MyDictionary!");

var myDict = new MyDictionary<Guid, Person>();
var personGenerator = new PersonsGenerator(100);

var defaultGuid = personGenerator.Persons.First().TransportId;
foreach (var person in personGenerator.Persons)
{
    myDict[person.TransportId] = person;
}

int count = 0;
foreach (var item in myDict)
{
    Console.WriteLine($"Key: {item.Key}, {item.Value}");
    count++;
}

Console.WriteLine(count);