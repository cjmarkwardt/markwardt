namespace Markwardt;

public static class ContainerTargetBuilderUtils
{
    public static IServiceBuilder CreateResolveBuilder(this IServiceContainer container, Type service)
        => new ContainerTargetBuilder(container, service);
}

public class ContainerTargetBuilder : IServiceBuilder
{
    public ContainerTargetBuilder(IServiceContainer container, Type service)
    {
        this.container = container;
        this.service = service;
    }

    private readonly IServiceContainer container;
    private readonly Type service;

    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await container.Resolve(service);
}