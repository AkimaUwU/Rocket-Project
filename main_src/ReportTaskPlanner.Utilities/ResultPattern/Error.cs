namespace ReportTaskPlanner.Utilities.ResultPattern;

public sealed record Error
{
    public string Message { get; }

    public Error(string message) => Message = message;

    public static Error None() => new Error(string.Empty);
}
