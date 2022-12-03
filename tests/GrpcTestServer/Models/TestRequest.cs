namespace GrpcTestServer.Models;

[ProtoContract]
public sealed record TestRequest
{
    [ProtoMember(1)]
    public string? Ping { get; init; }
}
