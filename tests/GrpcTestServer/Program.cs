using NKZSoft.Service.Configuration.Grpc.Extensions;

#pragma warning disable MA0036
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpcServer();

var app = builder.Build();

app.MapGrpcEndpoints<TestService>();
await app.RunAsync().ConfigureAwait(false);

/// <summary>
/// We need public access to the class for tests
/// </summary>
public partial class Program;
#pragma warning restore MA0036
