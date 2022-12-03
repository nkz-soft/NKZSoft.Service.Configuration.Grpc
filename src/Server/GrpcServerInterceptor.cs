namespace NKZSoft.Service.Configuration.Grpc.Server;

using Exceptions;

public class GrpcServerExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (Exception ex)
        {
            throw GrpcExceptionHelper.PrepareServerException(ex);
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.ClientStreamingServerHandler(requestStream, context, continuation);
        }
        catch (Exception ex)
        {
            throw GrpcExceptionHelper.PrepareServerException(ex);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }
        catch (Exception ex)
        {
            throw GrpcExceptionHelper.PrepareServerException(ex);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }
        catch (Exception ex)
        {
            throw GrpcExceptionHelper.PrepareServerException(ex);
        }
    }
}
