namespace NKZSoft.Service.Configuration.Grpc.Extensions;

/// <summary>
/// Provides extensions for the <see cref="IEndpointRouteBuilder"/> class to map GRPC endpoints for a specified service type.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the GRPC endpoints for the specified service type.
    /// </summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <param name="endpointRouteBuilder">The endpoint route builder.</param>
    /// <returns>The updated endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapGrpcEndpoints<T>(this IEndpointRouteBuilder endpointRouteBuilder) where T : class
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        endpointRouteBuilder.MapCodeFirstGrpcReflectionService();
        endpointRouteBuilder.MapGrpcService<T>();

        return endpointRouteBuilder;
    }
}
