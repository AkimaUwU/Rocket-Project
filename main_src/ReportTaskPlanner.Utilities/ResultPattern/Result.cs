namespace ReportTaskPlanner.Utilities.ResultPattern;

public abstract record Result
{
    public Error Error { get; }
    public bool IsSuccess { get; }

    public Result(Error error, bool IsSuccess)
    {
        if (string.IsNullOrWhiteSpace(error.Message) && !IsSuccess)
            throw new ArgumentException("Failure result should have message");
        if (!string.IsNullOrWhiteSpace(error.Message) && IsSuccess)
            throw new AggregateException("Success result shouldn't have error");
        Error = error;
        IsSuccess = IsSuccess;
    }

    public Result ToError()
    {
        if (IsSuccess)
            throw new ApplicationException("Result is success");
        return new FailureResult(Error.Message);
    }
}

public sealed record FailureResult : Result
{
    public FailureResult(string message)
        : base(new Error.Factory(message).Create(), false) { }
}

public sealed record SuccessResult : Result
{
    public SuccessResult()
        : base(new Error.Factory("").Create(), true) { }
}
