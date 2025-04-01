using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData.Decorators;

public sealed class TimeZoneDbLoggingDecorator(
    ITimeZoneDbRepository repository,
    Serilog.ILogger logger
) : ITimeZoneDbRepository
{
    private readonly ITimeZoneDbRepository _repository = repository;
    private readonly Serilog.ILogger _logger = logger;
    private const string ErrorTemplate = "{Context} {Operation} {Error}";
    private const string SuccessTemplate = "{Context} {Operation} {Info}";

    public async Task<Result> Save(TimeZoneDbOptions options)
    {
        Result result = await _repository.Save(options);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Error(
                    ErrorTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Save),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    SuccessTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Save),
                    options.Token
                )
            )
            .BackToResult();
    }

    public async Task<Result<TimeZoneDbOptions>> Get()
    {
        Result<TimeZoneDbOptions> result = await _repository.Get();
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Error(
                    ErrorTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Get),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    SuccessTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Get),
                    result.Value.Token
                )
            )
            .BackToResult();
    }

    public async Task<Result> Update(TimeZoneDbOptions options)
    {
        Result result = await _repository.Update(options);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Error(
                    ErrorTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Update),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    SuccessTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Update),
                    options.Token
                )
            )
            .BackToResult();
    }

    public async Task<Result> Delete(string token)
    {
        Result result = await _repository.Delete(token);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Error(
                    ErrorTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Update),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    SuccessTemplate,
                    nameof(ITimeZoneDbRepository),
                    nameof(Update),
                    token
                )
            )
            .BackToResult();
    }
}
