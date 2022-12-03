using NKZSoft.Service.Configuration.Grpc.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpcServer();

var app = builder.Build();

app.MapGrpcEndpoints<TestService>();

app.Run();

//We need public access to the class for tests
public partial class Program {}
