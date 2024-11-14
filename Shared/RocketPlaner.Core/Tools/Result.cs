namespace RocketPlaner.Core.Tools;

public class Result<T>
{
    public Error Error { get; init; }

    public T Value { get; init; }

    public bool IsError { get; init; }

    public bool IsOk { get; init; }

    private Result(Error error)
    {
        IsError = true;
        IsOk = false;
        Error = error;
        Value = default!;
    }

    private Result(T value)
    {
        IsError = false;
        IsOk = true;
        Error = Error.None;
        Value = value;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Fail(Error error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => Fail(error);

    public static implicit operator T(Result<T> result) => result.Value;
}
