using System.Net;

namespace Products.Library.Contracts.Helpers;

public class OperationResult<T>
{
    public T? Result { get; private set; }
    public List<string> Errors { get; } = [];
    public HttpStatusCode? StatusCode { get; private set; }
    public bool Success { get; private set; }

    public OperationResult<T> AddResult(T? result)
    {
        Result = result;
        StatusCode = HttpStatusCode.OK;
        Success = true;
        return this;
    }

    public OperationResult<T> AddError(
        string error,
        T? result = default,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        if (!string.IsNullOrWhiteSpace(error))
            Errors.Add(error);

        Result = result;
        StatusCode = statusCode;
        Success = false;
        return this;
    }

    public OperationResult<T> AddNotFound(string error)
        => AddError(error, statusCode: HttpStatusCode.NotFound);
}
