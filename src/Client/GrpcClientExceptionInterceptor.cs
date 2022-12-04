namespace NKZSoft.Service.Configuration.Grpc.Client;

using Exceptions;

public class GrpcClientExceptionInterceptor : Interceptor
{
    private readonly IErrorHandlingMethod _errorHandlingMethod;

    protected GrpcClientExceptionInterceptor(IErrorHandlingMethod errorHandlingMethod)
    {
        _errorHandlingMethod = errorHandlingMethod;
    }

    public static GrpcClientExceptionInterceptor Create(ErrorHandlingMethod errorHandlingMethod = ErrorHandlingMethod.ThrowException) =>
        new GrpcClientExceptionInterceptor(
            errorHandlingMethod switch
            {
                ErrorHandlingMethod.ThrowException => new ThrowExceptionHandler(),
                ErrorHandlingMethod.ReturnResult => new ResultExceptionHandler(),
                _ => throw new ArgumentException("Invalid enum value", nameof(errorHandlingMethod))
            });

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)

    {
        try
        {
            return base.UnaryServerHandler(request, context, continuation);
        }
        catch (RpcException ex)
        {
            return _errorHandlingMethod.Handle<TRequest, TResponse>(ex) ?? throw ex;
        }
    }

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
            return _errorHandlingMethod.Handle<TRequest, TResponse>(ex) ?? throw ex;
        }
    }

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
