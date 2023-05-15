namespace Markwardt;

public static class CreatorTargetBuilderUtils
{
    public static IServiceBuilder CreateBuilder(this IServiceCreator creator, Type service)
        => new CreatorTargetBuilder(creator, service);
}

public class CreatorTargetBuilder : IServiceBuilder
{
    public CreatorTargetBuilder(IServiceCreator creator, Type service)
    {
        this.creator = creator;
        this.service = service;
    }

    private readonly IServiceCreator creator;
    private readonly Type service;

    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await creator.Create(service, arguments, typeArguments);
}