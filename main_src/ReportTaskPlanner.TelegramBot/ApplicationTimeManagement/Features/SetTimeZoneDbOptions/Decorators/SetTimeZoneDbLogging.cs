using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions.Decorators;

public sealed class SetTimeZoneDbLogging
    : ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>
{
    private readonly ILogger _logger;
    private readonly ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions> _handler;

    public SetTimeZoneDbLogging(
        ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions> handler,
        ILogger logger
    )
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<Result<TimeZoneDbOptions>> Handle(SetTimeZoneDbOptionsCommand command)
    {
        Result<TimeZoneDbOptions> result = await _handler.Handle(command);
        if (result.IsFailure)
            _logger.LogError(result.Error, nameof(SetTimeZoneDbOptionsCommand));
        else
            _logger.Information(
                "{Context}. Time Zone Db Option Token Key has been set: {Token}",
                nameof(SetTimeZoneDbLogging),
                command.Token
            );
        return result;
    }
}
