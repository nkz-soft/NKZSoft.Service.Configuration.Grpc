namespace GrpcTestServer.Services;

using Models;

[System.ServiceModel.ServiceContract]
public interface ITestService
{
    [OperationContract]
    ValueTask<TestResponse> Ping(TestRequest request, CancellationToken cancellationToken = default);
}
