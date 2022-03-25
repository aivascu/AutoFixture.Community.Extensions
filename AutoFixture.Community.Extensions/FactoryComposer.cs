using AutoFixture.Kernel;

namespace AutoFixture;

public class FactoryComposer<T> : IFactoryComposer<T>
{
    public FactoryComposer(IRequestSpecification specification, ISpecimenBuilder builder)
    {
        Specification = specification ?? throw new ArgumentNullException(nameof(specification));
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public FactoryComposer(ISpecimenBuilder builder)
        : this(CreateSpecification(), Compose(CreateBuilder(new ModestConstructorQuery()), builder))
    {
    }

    public IRequestSpecification Specification { get; }
    public ISpecimenBuilder Builder { get; }

    public IFactoryComposer<T> SelectedBy(IMethodQuery selector)
    {
        var builder = Replace(
            CreateBuilder(selector),
            new IsFactoryInvokerNode(typeof(T)));
        return new FactoryComposer<T>(Specification, builder);
    }

    public IFactoryComposer<T> WithParameter<TParameter>(string parameterName, TParameter value)
    {
        return WithParameter(
            new ParameterSpecification(typeof(TParameter), parameterName),
            new FixedBuilder(value));
    }

    public IFactoryComposer<T> WithParameterFactory<TParameter>(string parameterName, Func<TParameter> value)
    {
        return WithParameter(
            new ParameterSpecification(typeof(TParameter), parameterName),
            new SpecimenFactory<TParameter>(value));
    }

    public IFactoryComposer<T> WithParameterFactory<TInput, TParameter>(string parameterName,
        Func<TInput, TParameter> value)
    {
        return WithParameter(
            new ParameterSpecification(typeof(TParameter), parameterName),
            new SpecimenFactory<TInput, TParameter>(value));
    }

    public IFactoryComposer<T> WithParameterFactory<TParameter>(Func<TParameter> value)
    {
        return WithParameter(
            new ParameterSpecification(
                new ParameterTypeCriterion(typeof(TParameter))),
            new SpecimenFactory<TParameter>(value));
    }

    public IFactoryComposer<T> WithParameterFactory<TInput, TParameter>(Func<TInput, TParameter> value)
    {
        return WithParameter(
            new ParameterSpecification(
                new ParameterTypeCriterion(typeof(TParameter))),
            new SpecimenFactory<TInput, TParameter>(value));
    }

    public IFactoryComposer<T> WithParameter<TParameter>(TParameter value)
    {
        return WithParameter(
            new ParameterSpecification(
                new ParameterTypeCriterion(typeof(TParameter))),
            new FixedBuilder(value));
    }

    public IFactoryComposer<T> WithParameter(IRequestSpecification specification, ISpecimenBuilder valueBuilder)
    {
        var builder = Compose(
            new FilteringSpecimenBuilder(valueBuilder, specification),
            Builder);

        return new FactoryComposer<T>(Specification, builder);
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (!Specification.IsSatisfiedBy(request))
            return new NoSpecimen();

        var currentContext = new ContextChain(new SpecimenContext(Builder), context);
        var result = Builder.Create(request, currentContext);
        return result;
    }

    private ISpecimenBuilder Replace(ISpecimenBuilder builder, IRequestSpecification specification)
    {
        return Builder is IEnumerable<ISpecimenBuilder> composite
            ? new CompositeSpecimenBuilder(ReplaceNode(composite))
            : Builder;

        IEnumerable<ISpecimenBuilder> ReplaceNode(IEnumerable<ISpecimenBuilder> builders)
        {
            foreach (var item in builders)
                if (specification.IsSatisfiedBy(item))
                    yield return builder;
        }
    }

    private static ISpecimenBuilder Compose(params ISpecimenBuilder[] builders)
    {
        return new CompositeSpecimenBuilder(builders.SelectMany(Unwrap));

        IEnumerable<ISpecimenBuilder> Unwrap(ISpecimenBuilder source)
        {
            if (source is CompositeSpecimenBuilder composite)
            {
                foreach (var builder in composite)
                    yield return builder;
            }
            else
                yield return source;
        }
    }

    private static IRequestSpecification CreateSpecification()
    {
        return new OrRequestSpecification(
            new ExactTypeSpecification(typeof(T)),
            new SeedRequestSpecification(typeof(T)));
    }

    private static ISpecimenBuilder CreateBuilder(IMethodQuery query)
    {
        return new CompositeSpecimenBuilder(
            new FilteringSpecimenBuilder(
                new SeedIgnoringRelay(),
                new SeedRequestSpecification(typeof(T))),
            new FilteringSpecimenBuilder(
                new MethodInvoker(query),
                new ExactTypeSpecification(typeof(T))),
            new PropertyRequestRelay());
    }
}
