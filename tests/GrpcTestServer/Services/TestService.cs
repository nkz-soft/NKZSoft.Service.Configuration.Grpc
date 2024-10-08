﻿namespace GrpcTestServer.Services;

using Models;

internal sealed class TestService : ITestService
{
    private const string TestString = "Test";

    public async ValueTask<TestResponse> Ping(TestRequest request, CancellationToken cancellationToken = default)
    {
        if (!string.Equals(request.Ping, TestString, StringComparison.Ordinal))
        {
            throw new ArgumentException($"The ping argument has an invalid value {request.Ping}.", nameof(request));
        }

        return await Task.FromResult(new TestResponse { Pong = request.Ping }).ConfigureAwait(false);
    }
}
