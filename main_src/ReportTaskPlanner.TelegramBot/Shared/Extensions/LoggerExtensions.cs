using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.Shared.Extensions;

public static class LoggerExtensions
{
    public static void LogError(this ILogger logger, Error error, string context) =>
        logger.Error("{Context}. Error: {Message}.", context, error);
}
