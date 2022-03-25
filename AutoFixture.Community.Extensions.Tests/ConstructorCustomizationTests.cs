using AutoFixture.Community.Extensions.Tests.TestTypes;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.Community.Extensions.Tests;

public class ConstructorCustomizationTests
{
[Fact]
public void CreateUsingGreedyConstructorWithParameters()
{
    var fixture = new Fixture();

    fixture.Customize<Person>(c => c
        .FromFactory(b => b
            .SelectedBy(new GreedyConstructorQuery())
            .WithParameter("firstName", "Duke")
            .WithParameter("lastName", "Nukem")
        ));

    var instance = fixture.Create<Person>();

    Assert.Equal("Duke Nukem", instance.Name);
}

    [Fact]
    public void CreatesUsingExplicitModestConstructor()
    {
        var fixture = new Fixture();

        fixture.Customize<Person>(c => c
            .FromFactory(b => b
                .SelectedBy(new ModestConstructorQuery())
                .WithParameter("Boris")
            ));

        var instance = fixture.Create<Person>();

        Assert.Equal("Boris", instance.Name);
    }

    [Fact]
    public void CreatesUsingImplicitConstructorQuery()
    {
        var fixture = new Fixture();

        fixture.Customize<Person>(c => c
            .FromFactory(b => b
                .WithParameter("James")
            ));

        var instance = fixture.Create<Person>();

        Assert.Equal("James", instance.Name);
    }

    [Fact]
    public void CreatesUsingNamedParameterFactory()
    {
        var fixture = new Fixture();

        fixture.Customize<Person>(c => c
            .FromFactory(b => b
                .WithParameterFactory("name", () => $"Lana {123}")
            ));

        var instance = fixture.Create<Person>();

        Assert.Equal("Lana 123", instance.Name);
    }

    [Fact]
    public void CreatesUsingParameterFactory()
    {
        var fixture = new Fixture();
        var val = 1;

        fixture.Customize<Person>(c => c
            .FromFactory(b => b
                .SelectedBy(new GreedyConstructorQuery())
                .WithParameterFactory(() => $"Dun{++val}")
            ));

        var instance = fixture.Create<Person>();

        Assert.Equal("Dun2 Dun3", instance.Name);
    }
}
