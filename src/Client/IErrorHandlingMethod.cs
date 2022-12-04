namespace NKZSoft.Service.Configuration.Grpc.Client;

public interface IErrorHandlingMethod
{
    Task<TResponse>? Handle<TRequest, TResponse>(RpcException ex);
}
