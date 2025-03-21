namespace ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public Error Error { get; }

    protected Result(Error error)
    {
        IsSuccess = false;
        IsFailure = true;
        Error = error;
    }

    protected Result()
    {
        IsSuccess = true;
        IsFailure = false;
        Error = Error.None;
    }

    public static Result Success() => new Result();

    public static Result Failure(Error error) => new Result(error);

    public static implicit operator Result(Error error) => Failure(error);
}

public sealed class Result<T> : Result
{
    private readonly T _value;

    public T Value =>
        IsFailure ? throw new ApplicationException("Cannot access failure result") : _value;

    private Result(T value) => _value = value;

    private Result(Error error)
        : base(error) => _value = default!;

    public static Result<T> Success(T value) => new(value);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => new Result<T>(error);

    public static implicit operator T(Result<T> result) => result.Value;
}
