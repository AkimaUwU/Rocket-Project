namespace ReportTaskPlanner.Utilities.ResultPattern;

public abstract record TypedResult<T>
{
    private readonly T _value;
    public Error Error { get; }
    public bool IsSuccess { get; }

    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new ApplicationException("Cannot access to failure result value");
            return _value;
        }
    }

    public TypedResult(T value, Error error, bool isSuccess)
    {
        if (string.IsNullOrWhiteSpace(error.Message) && !isSuccess)
            throw new ArgumentException("Failure result should have message");
        if (!string.IsNullOrWhiteSpace(error.Message) && isSuccess)
            throw new AggregateException("Success result shouldn't have error");
        _value = value;
        Error = error;
        IsSuccess = isSuccess;
    }

    public TypedResult<U> ToError<U>()
    {
        if (IsSuccess)
            throw new ApplicationException("Result is success");
        return new FailureTypedResult<U>(Error.Message);
    }
}

public sealed record SuccessTypedResult<T> : TypedResult<T>
{
    public SuccessTypedResult(T value)
        : base(value, new Error.Factory("").Create(), true) { }
}

public sealed record FailureTypedResult<T> : TypedResult<T>
{
    public FailureTypedResult(string message)
        : base(default, new Error.Factory(message).Create(), false) { }
}
