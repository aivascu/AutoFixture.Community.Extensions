namespace AutoFixture.Community.Extensions.Tests.TestTypes;

public class Person
{
    public Person(string name)
    {
        Name = name;
    }

    public Person(string firstName, string lastName)
    {
        Name = $"{firstName} {lastName}";
    }

    public string Name { get; }
}
