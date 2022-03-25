using System.Collections;

namespace AutoFixture.Kernel;

internal class ContextChain : ISpecimenContext, IEnumerable<ISpecimenContext>
{
    private readonly List<ISpecimenContext> contexts;

    public ContextChain(params ISpecimenContext[] contexts)
        : this(contexts.AsEnumerable())
    {
    }

    public ContextChain(IEnumerable<ISpecimenContext> contexts)
    {
        if (contexts is null) throw new ArgumentNullException(nameof(contexts));
        this.contexts = contexts.ToList();
    }

    public IEnumerator<ISpecimenContext> GetEnumerator()
    {
        return contexts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public object Resolve(object request)
    {
        return contexts
            .Select(context => context.Resolve(request))
            .FirstOrDefault(result => result is not NoSpecimen)!;
    }
}
