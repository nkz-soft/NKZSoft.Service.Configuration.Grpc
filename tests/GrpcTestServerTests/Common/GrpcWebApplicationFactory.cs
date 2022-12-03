namespace GrpcTestServerTests.Common;

using NKZSoft.Service.Configuration.Grpc.Client;

public sealed class GrpcWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public T CreateGrpcClientService<T>() where T : class
    {
        var client = CreateClient();

        var grpcChannel =  GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = client,
        });
        var callInvoker = grpcChannel.Intercept(new GrpcClientExceptionInterceptor());
        return callInvoker.CreateGrpcService<T>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Test");
    }
}
