using AutoFixture.Kernel;

namespace AutoFixture;

public interface IFactoryComposer<T> : ISpecimenBuilder
{
    IFactoryComposer<T> SelectedBy(IMethodQuery selector);
    IFactoryComposer<T> WithParameter(IRequestSpecification specification, ISpecimenBuilder valueBuilder);
    IFactoryComposer<T> WithParameter<TParameter>(TParameter value);
    IFactoryComposer<T> WithParameterFactory<TParameter>(Func<TParameter> value);
    IFactoryComposer<T> WithParameterFactory<TInput, TParameter>(Func<TInput, TParameter> value);
    IFactoryComposer<T> WithParameter<TParameter>(string parameterName, TParameter value);
    IFactoryComposer<T> WithParameterFactory<TParameter>(string parameterName, Func<TParameter> value);
    IFactoryComposer<T> WithParameterFactory<TInput, TParameter>(string parameterName, Func<TInput, TParameter> value);
}
