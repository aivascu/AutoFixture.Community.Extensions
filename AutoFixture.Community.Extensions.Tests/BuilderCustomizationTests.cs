using AutoFixture.Community.Extensions.Tests.TestTypes;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.Community.Extensions.Tests;

public class BuilderCustomizationTests
{
    [Fact]
    public void CreateUsingGreedyConstructorWithParameters()
    {
        var fixture = new Fixture();

        var instance = fixture
            .Build<Person>()
            .FromFactory(c => c
                .SelectedBy(new GreedyConstructorQuery())
                .WithParameter("firstName", "Duke")
                .WithParameter("lastName", "Nukem"))
            .Create();

        Assert.Equal("Duke Nukem", instance.Name);
    }

    [Fact]
    public void CreatesUsingExplicitModestConstructor()
    {
        var fixture = new Fixture();

        var instance = fixture
            .Build<Person>()
            .FromFactory(c => c
                .SelectedBy(new ModestConstructorQuery())
                .WithParameter("Boris"))
            .Create();

        Assert.Equal("Boris", instance.Name);
    }

    [Fact]
    public void CreatesUsingImplicitConstructorQuery()
    {
        var fixture = new Fixture();

        var instance = fixture
            .Build<Person>()
            .FromFactory(c => c
                .WithParameter("James"))
            .Create();

        Assert.Equal("James", instance.Name);
    }

    [Fact]
    public void CreatesUsingNamedParameterFactory()
    {
        var fixture = new Fixture();

        var instance = fixture
            .Build<Person>()
            .FromFactory(c => c
                .WithParameterFactory("name", () => $"Lana {123}"))
            .Create();

        Assert.Equal("Lana 123", instance.Name);
    }

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

    [Fact]
    public void CreatesUsingParameterFactory()
    {
        var fixture = new Fixture();
        var val = 1;

        var instance = fixture
            .Build<Person>()
            .FromFactory(c => c
                .SelectedBy(new GreedyConstructorQuery())
                .WithParameterFactory(() => $"Dun{++val}"))
            .Create();

        Assert.Equal("Dun2 Dun3", instance.Name);
    }
}
