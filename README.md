# AutoFixture.Community.Extensions

A package designed to be a staging area for ideas to be integrated into AutoFixture, without affecting the AutoFixture public contracts.

The APIs in this library are making heavy use of the AutoFixture extension points, but might also introduce their own abstractions over the library.

## Features

### Constructor customization

This feature makes use of the `FromFactory(ISpecimenBuilder)` in the `ICustomizationComposer<T>` to inject the constructor customization node.

```c#
[Fact]
public void CreateUsingGreedyConstructorWithParameters()
{
    var fixture = new Fixture();

    fixture.Customize<Person>(c => c
        .FromFactory(b => b
            .SelectedBy(new GreedyConstructorQuery())
            .WithParameter("firstName", "John")
            .WithParameter("lastName", "Doe")
        ));

    var instance = fixture.Create<Person>();

    Assert.Equal("John Doe", instance.FullName);
}
```

The feature also supports building objects with one time constructor customization.

```c#
[Fact]
public void CreatesUsingNamedParameterFactory1()
{
    var fixture = new Fixture();

    fixture.Inject("Lang");

    var instance = fixture
        .Build<Person>()
        .FromFactory(c => c
            .WithParameterFactory("name", (string st) => $"Lana {st}"))
        .Create();

    Assert.Equal("Lana Lang", instance.Name);
}
```
