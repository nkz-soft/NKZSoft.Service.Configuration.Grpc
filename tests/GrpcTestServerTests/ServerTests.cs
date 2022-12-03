namespace GrpcTestServerTests;

using Common;
using GrpcTestServer.Models;

[Collection(nameof(GrpcCollectionDefinition))]
public class ServerTests
{
    private const string TestString = "Test";
    private const string WrongTestString = "Test1";

    private readonly GrpcWebApplicationFactory<Program> _factory;
    private readonly ITestService _clientService;

    public ServerTests(GrpcWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _clientService = _factory.CreateGrpcClientService<ITestService>();
    }

    [Fact]
    public async Task TestPing()
    {
        var result = await _clientService.Ping(new TestRequest() { Ping = TestString });

        result.Should().NotBeNull();
        result.Pong.Should().Be(TestString);
    }

    [Fact]
    public async Task TestPingException()
    {
        async Task Act() => await _clientService.Ping(new TestRequest() { Ping = WrongTestString });

        var exception = await Assert.ThrowsAsync<RpcException>(Act);

        exception.InnerException.Should().NotBeNull();
        exception.InnerException!.Message.Should().NotBeNull();
        exception.InnerException.Message.Should().StartWith("The ping argument has an invalid value");
    }
}


