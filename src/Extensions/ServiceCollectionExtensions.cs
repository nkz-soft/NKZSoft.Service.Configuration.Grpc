namespace NKZSoft.Service.Configuration.Grpc.Extensions;

using Client;
using Server;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcServer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCodeFirstGrpcReflection();
        services.AddCodeFirstGrpc(opt => opt.Interceptors.Add<GrpcServerExceptionInterceptor>());
        return services;
    }

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
