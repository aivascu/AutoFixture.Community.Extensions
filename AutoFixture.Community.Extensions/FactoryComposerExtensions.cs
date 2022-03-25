using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture;

public static class FactoryComposerExtensions
{
    public static IPostprocessComposer<T> FromFactory<T>(
        this ICustomizationComposer<T> source,
        Func<IFactoryComposer<T>, IFactoryComposer<T>> configure)
    {
        return source.FromFactory(configure(new FactoryComposer<T>(source)));
    }

    public static IFactoryComposer<T> UsingGreedyConstructor<T>(this IFactoryComposer<T> source)
    {
        return source.SelectedBy(new GreedyConstructorQuery());
    }

    public static IFactoryComposer<T> UsingModestConstructor<T>(this IFactoryComposer<T> source)
    {
        return source.SelectedBy(new ModestConstructorQuery());
    }
}
