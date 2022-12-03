namespace NKZSoft.Service.Configuration.Grpc.Exceptions;

internal sealed class TreatResponseStream<TResponse> : IAsyncStreamReader<TResponse>
{
    private readonly IAsyncStreamReader<TResponse> _stream;

    public TreatResponseStream(IAsyncStreamReader<TResponse> stream) => _stream = stream;

    public TResponse Current => _stream.Current;

    public async Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        try
        {
            return await _stream.MoveNext(cancellationToken).ConfigureAwait(false);
        }
        catch (RpcException ex)
        {
            GrpcExceptionHelper.PrepareClientException(ex);
            throw;
        }
    }
}
