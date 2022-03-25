namespace AutoFixture.Kernel;

internal class IsFactoryInvokerNode : IRequestSpecification
{
    public IsFactoryInvokerNode(Type target)
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    public Type Target { get; }

    public bool IsSatisfiedBy(object request)
    {
        return request is FilteringSpecimenBuilder
        {
            Builder: MethodInvoker,
            Specification: ExactTypeSpecification typeSpecification
        } && typeSpecification.TargetType == Target;
    }
}
