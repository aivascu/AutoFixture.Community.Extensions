using System.Reflection;

namespace AutoFixture.Kernel;

internal class ParameterTypeCriterion : IEquatable<ParameterInfo>
{
    public ParameterTypeCriterion(Type type)
        : this(new Criterion<Type>(type, EqualityComparer<Type>.Default))
    {
    }

    public ParameterTypeCriterion(IEquatable<Type> typeCriterion)
    {
        TypeCriterion = typeCriterion ?? throw new ArgumentNullException(nameof(typeCriterion));
    }

    public IEquatable<Type> TypeCriterion { get; }

    public bool Equals(ParameterInfo? other)
    {
        return other is not null
               && TypeCriterion.Equals(other.ParameterType);
    }
}
