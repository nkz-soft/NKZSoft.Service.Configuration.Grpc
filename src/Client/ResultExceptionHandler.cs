namespace NKZSoft.Service.Configuration.Grpc.Client;

internal class ResultExceptionHandler : IErrorHandlingMethod
{
    public T Handle(RpcException ex) => throw new NotImplementedException();
}
