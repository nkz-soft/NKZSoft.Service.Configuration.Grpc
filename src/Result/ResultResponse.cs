namespace NKZSoft.Service.Configuration.Grpc.Result;

[ProtoContract]
public record ResultResponse
{
    public ResultResponse(bool isSuccess, IEnumerable<ErrorResponse> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors.ToArray();
    }

    protected ResultResponse()
    {
    }

    [ProtoMember(1)]
    public bool IsSuccess { get; init; }


    [ProtoMember(2)]
    public ErrorResponse[] Errors { get; init; } = Array.Empty<ErrorResponse>();
}

