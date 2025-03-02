namespace ReportTaskPlanner.Utilities.ResultPattern;

public sealed record Error
{
    public string Message { get; }

    private Error()
    {
        Message = string.Empty;
    }

    private Error(string? message)
    {
        Message = string.IsNullOrWhiteSpace(message) ? "" : message;
    }

    public sealed record Factory(string? message)
    {
        public Error Create()
        {
            return new Error(message);
        }
    }
}
