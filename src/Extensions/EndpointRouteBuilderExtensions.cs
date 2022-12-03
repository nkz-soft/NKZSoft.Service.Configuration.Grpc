namespace NKZSoft.Service.Configuration.Grpc.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGrpcEndpoints<T>(this IEndpointRouteBuilder endpointRouteBuilder) where T : class
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        endpointRouteBuilder.MapCodeFirstGrpcReflectionService();
        endpointRouteBuilder.MapGrpcService<T>();

        return endpointRouteBuilder;
    }
}
