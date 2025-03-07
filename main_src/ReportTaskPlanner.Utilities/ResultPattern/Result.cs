namespace ReportTaskPlanner.Utilities.ResultPattern;

public record Result
{
    public bool IsSuccess { get; protected init; }
    public Error Error { get; protected init; }

    public static Result Success() => new Result() { IsSuccess = true, Error = Error.None() };

    public static Result Failure(Error error) => new Result() { IsSuccess = false, Error = error };

    public static Result Failure(string message) =>
        new Result() { IsSuccess = false, Error = new Error(message) };

    public static implicit operator Result(Error error) => Failure(error);
}

public record Result<TValue> : Result
{
    private readonly TValue _value = default!;
    public TValue Value
    {
        get =>
            IsSuccess
                ? _value
                : throw new ApplicationException("Cannot access body of failure result");
        private init => _value = value;
    }

    public static Result<U> Success<U>(U value) =>
        new Result<U>()
        {
            IsSuccess = true,
            Error = Error.None(),
            Value = value,
        };

    public static Result<U> Failure<U>(Error error) =>
        new Result<U>()
        {
            IsSuccess = false,
            Error = error,
            Value = default!,
        };

    public static Result<U> Failure<U>(string message) =>
        new Result<U>()
        {
            IsSuccess = false,
            Error = new Error(message),
            Value = default!,
        };

    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);

    public static implicit operator Result<TValue>(TValue value) => Success<TValue>(value);

    public static implicit operator TValue(Result<TValue> result) => result.Value;
}
