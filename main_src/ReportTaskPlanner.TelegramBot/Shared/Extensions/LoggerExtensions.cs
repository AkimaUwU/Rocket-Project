using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.Shared.Extensions;

public sealed record LoggerChunk<T>(ILogger Logger, Option<T> Option);

public static class LoggerExtensions
{
    public static void LogError(this ILogger logger, Error error, string context) =>
        logger.Error("{Context}. Error: {Message}.", context, error);

    public static LoggerChunk<T> ToLoggerChunk<T>(this Option<T> option, ILogger logger) =>
        new LoggerChunk<T>(logger, option);

    public static LoggerChunk<T> LogOnHasValue<T>(this LoggerChunk<T> chunk, Action<ILogger> action)
    {
        if (chunk.Option.HasValue)
            action(chunk.Logger);
        return chunk;
    }

    public static LoggerChunk<T> LogOnNoValue<T>(this LoggerChunk<T> chunk, Action<ILogger> action)
    {
        if (!chunk.Option.HasValue)
            action(chunk.Logger);
        return chunk;
    }

    public static Option<T> BackToOption<T>(this LoggerChunk<T> chunk) => chunk.Option;
}
