namespace NKZSoft.Service.Configuration.Grpc.Exceptions;

using Converters;

internal static class GrpcExceptionHelper
{
    private const string MetadataExceptionStore = "exception-bin";

    private static JsonSerializerOptions JsonSerializerOptions
        => new JsonSerializerOptions
        {
            Converters = { new ExceptionConverter() }
        };

    public static RpcException PrepareServerException(Exception ex)
    {
        var exception = JsonSerializer.Serialize(ex, JsonSerializerOptions);
        var exceptionByteArray = Encoding.UTF8.GetBytes(exception);

        var metadata = new Metadata();
        metadata.Add(MetadataExceptionStore, exceptionByteArray);

        return new RpcException(new Status(StatusCode.Internal, "Error"), metadata);
    }

    public static void PrepareClientException(RpcException ex)
    {
        if (!ex.Trailers.Any(x => x.Key.Equals(MetadataExceptionStore, StringComparison.Ordinal)))
        {
            return;
        }

        var bytesValue= ex.Trailers.GetValueBytes(MetadataExceptionStore);

        if (bytesValue is null)
        {
            return;
        }

        var exceptionString = Encoding.UTF8.GetString(bytesValue);

        var exception = JsonSerializer.Deserialize<Exception>(exceptionString, JsonSerializerOptions);

        if (exception is null)
        {
            return;
        }

        ex.GetType().BaseType?
            .GetField("_innerException", BindingFlags.NonPublic | BindingFlags.Instance)?
            .SetValue(ex, exception);
    }
}
