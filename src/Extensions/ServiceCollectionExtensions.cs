namespace NKZSoft.Service.Configuration.Grpc.Extensions;

using Client;
using Server;

/// <summary>
/// Contains extension methods for IServiceCollection to add Grpc server and client functionality.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Grpc server functionality to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add Grpc server functionality to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddGrpcServer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCodeFirstGrpcReflection();
        services.AddCodeFirstGrpc(opt => opt.Interceptors.Add<GrpcServerExceptionInterceptor>());
        return services;
    }

    /// <summary>
    /// Adds Grpc client functionality to the IServiceCollection.
    /// </summary>
    /// <typeparam name="T">The type of the Grpc client.</typeparam>
    /// <param name="services">The IServiceCollection to add Grpc client functionality to.</param>
    /// <param name="configureClient">The configuration options for the Grpc client.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddGrpcClient<T>(this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configureClient) where T : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureClient);

        services.AddCodeFirstGrpcClient<T>(configureClient)
                .AddInterceptor<GrpcClientExceptionInterceptor>();

        services.AddSingleton<GrpcClientExceptionInterceptor>();

        return services;
    }
}
