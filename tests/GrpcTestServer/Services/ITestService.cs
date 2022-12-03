namespace GrpcTestServer.Services;

using Models;

[ServiceContract]
public interface ITestService
{
    [OperationContract]
    ValueTask<TestResponse> Ping(TestRequest request, CancellationToken cancellationToken = default);
}
