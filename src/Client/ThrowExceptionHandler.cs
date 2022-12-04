namespace NKZSoft.Service.Configuration.Grpc.Client;

internal class ThrowExceptionHandler : IErrorHandlingMethod
{
    public T Handle(RpcException ex) => throw new NotImplementedException();
}
