namespace NKZSoft.Service.Configuration.Grpc.Client;

using Exceptions;

/// <summary>
/// GrpcClientExceptionInterceptor is a class that intercepts and handles exceptions thrown by Grpc calls.
/// </summary>
public class GrpcClientExceptionInterceptor : Interceptor
{
    /// <summary>
    /// Overrides the UnaryServerHandler method to handle RpcExceptions.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="request">The request message.</param>
    /// <param name="context">The server call context.</param>
    /// <param name="continuation">The continuation of the server call.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return base.UnaryServerHandler(request, context, continuation);
        }
        catch (RpcException ex)
        {
            GrpcExceptionHelper.PrepareClientException(ex);
            throw;
        }
    }

    /// <summary>
    /// Overrides the BlockingUnaryCall method to handle RpcExceptions.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="request">The request message.</param>
    /// <param name="context">The interceptor context.</param>
    /// <param name="continuation">The continuation of the call.</param>
    /// <returns>The response message.</returns>
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return base.BlockingUnaryCall(request, context, continuation);
        }
        catch (RpcException ex)
        {
            GrpcExceptionHelper.PrepareClientException(ex);
            throw;
        }
    }

    /// <summary>
    /// Overrides the AsyncUnaryCall method to handle RpcExceptions.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="request">The request message.</param>
    /// <param name="context">The interceptor context.</param>
    /// <param name="continuation">The continuation of the call.</param>
    /// <returns>An instance of AsyncUnaryCall representing an asynchronous unary invocation.</returns>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return new AsyncUnaryCall<TResponse>(
            TreatResponseUnique(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    /// <summary>
    /// Overrides the AsyncClientStreamingCall method to handle RpcExceptions.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="context">The interceptor context.</param>
    /// <param name="continuation">The continuation of the call.</param>
    /// <returns>An instance of AsyncClientStreamingCall representing an asynchronous client-streaming invocation.</returns>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(context);
        return new AsyncClientStreamingCall<TRequest, TResponse>(
            call.RequestStream,
            TreatResponseUnique(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    /// <summary>
    /// Overrides the AsyncServerStreamingCall method to handle RpcExceptions.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="request">The request message.</param>
    /// <param name="context">The interceptor context.</param>
    /// <param name="continuation">The continuation of the call.</param>
    /// <returns>An instance of AsyncServerStreamingCall representing an asynchronous server-streaming invocation.</returns>
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return new AsyncServerStreamingCall<TResponse>(
            new TreatResponseStream<TResponse>(call.ResponseStream),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    /// <summary>
    /// Overrides the AsyncDuplexStreamingCall method to handle RpcExceptions.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="context">The interceptor context.</param>
    /// <param name="continuation">The continuation of the call.</param>
    /// <returns>An instance of AsyncDuplexStreamingCall representing an asynchronous duplex-streaming invocation.</returns>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest,
            TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(context);
        return new AsyncDuplexStreamingCall<TRequest, TResponse>(
            call.RequestStream,
            new TreatResponseStream<TResponse>(call.ResponseStream),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    private static async Task<TResponse> TreatResponseUnique<TResponse>(Task<TResponse> resposta)
    {
        try
        {
            return await resposta;
        }
        catch (RpcException ex)
        {
            GrpcExceptionHelper.PrepareClientException(ex);
            throw;
        }
    }
}
