namespace ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

public sealed record Error(string Message)
{
    public static Error None => new Error(string.Empty);
}
