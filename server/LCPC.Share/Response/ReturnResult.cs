namespace LCPC.Share.Response;

public class ReturnResult<T>
{
    public T Data { get; set; }
    public long TotalCount { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }

    public ReturnResult(bool success, T data)
    : this(success, data, null)

    {
        this.Data = data;
        this.Success = success;
    }

    public ReturnResult(bool success, T data, string message)
    {
        this.Success = success;
        this.Data = data;
        this.Message = message;
    }
}

public class ReturnResult : ReturnResult<object>
{
    public ReturnResult(bool success)
    : base(success, null)
    {

    }
    public ReturnResult(bool success, object data) : base(success, data)
    {
    }

    public ReturnResult(bool success, object data, string message) : base(success, data, message)
    {
    }
}