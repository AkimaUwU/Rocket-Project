using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.Shared.Extensions;

public sealed record OptionLoggingChunk<T>(ILogger Logger, Option<T> Option);

public sealed record ResultLoggingChunk<T>(ILogger Logger, Result<T> Result);

public sealed record ResultLoggingChunk(Result Result, ILogger Logger);

public static class LoggerExtensions
{
    public static OptionLoggingChunk<T> ToLoggerChunk<T>(this Option<T> option, ILogger logger) =>
        new(logger, option);

    public static ResultLoggingChunk<T> ToLoggerChunk<T>(this Result<T> result, ILogger logger) =>
        new(logger, result);

    public static ResultLoggingChunk ToLoggerChunk(this Result result, ILogger logger) =>
        new(result, logger);

    public static OptionLoggingChunk<T> LogOnHasValue<T>(
        this OptionLoggingChunk<T> chunk,
        Action<ILogger> action
    )
    {
        if (chunk.Option.HasValue)
            action(chunk.Logger);
        return chunk;
    }

    public static OptionLoggingChunk<T> LogOnNoValue<T>(
        this OptionLoggingChunk<T> chunk,
        Action<ILogger> action
    )
    {
        if (!chunk.Option.HasValue)
            action(chunk.Logger);
        return chunk;
    }

    public static ResultLoggingChunk<T> LogOnError<T>(
        this ResultLoggingChunk<T> chunk,
        Action<ILogger> action
    )
    {
        if (chunk.Result.IsFailure)
            action(chunk.Logger);
        return chunk;
    }

    public static ResultLoggingChunk LogOnError(
        this ResultLoggingChunk chunk,
        Action<ILogger> action
    )
    {
        if (chunk.Result.IsFailure)
            action(chunk.Logger);
        return chunk;
    }

    public static ResultLoggingChunk LogOnSuccess(
        this ResultLoggingChunk chunk,
        Action<ILogger> action
    )
    {
        if (chunk.Result.IsSuccess)
            action(chunk.Logger);
        return chunk;
    }

    public static void LogError(this ILogger logger, Error error, string context) =>
        logger.Error("{Context}. Error: {Message}.", context, error);

    public static ResultLoggingChunk<T> LogOnSuccess<T>(
        this ResultLoggingChunk<T> chunk,
        Action<ILogger> action
    )
    {
        if (chunk.Result.IsSuccess)
            action(chunk.Logger);
        return chunk;
    }

    public static Option<T> BackToOption<T>(this OptionLoggingChunk<T> chunk) => chunk.Option;

    public static Result<T> BackToResult<T>(this ResultLoggingChunk<T> chunk) => chunk.Result;

    public static Result BackToResult(this ResultLoggingChunk chunk) => chunk.Result;
}
