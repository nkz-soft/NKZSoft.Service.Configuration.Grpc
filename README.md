# NKZSoft.Service.Configuration.Grpc

This library contains the following components:
- Global error handlers 

## Global error handlers

Any exception on the server side is converted to an inner exception in RpcException with a populated Message field.

## Using 

On the server side:
```csharp

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpcServer();

var app = builder.Build();

app.MapGrpcEndpoints<TestService>();

app.Run();

```

On the client side:

```csharp
var grpcChannel =  GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions
{
    HttpClient = client,
});
var callInvoker = grpcChannel.Intercept(new GrpcClientExceptionInterceptor());
callInvoker.CreateGrpcService<T>();
```


