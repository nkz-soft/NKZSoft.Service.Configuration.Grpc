namespace GrpcTestServer.Models;

[ProtoContract]
public sealed record TestResponse
{
    [ProtoMember(1)]
    public string? Pong { get; init; }
}
