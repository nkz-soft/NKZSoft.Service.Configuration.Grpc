namespace GrpcTestServer.Models;

using NKZSoft.Service.Configuration.Grpc.Result;

[ProtoInclude(3, typeof(TestResponse))]
public sealed record TestResultResponse : ResultResponse
{
}
