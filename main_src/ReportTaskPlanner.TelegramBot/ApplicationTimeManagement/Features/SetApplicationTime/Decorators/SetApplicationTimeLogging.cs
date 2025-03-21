using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime.Decorators;

public sealed class SetApplicationTimeLogging
    : ICommandHandler<SetApplicationTimeCommand, ApplicationTime>
{
    private readonly ILogger _logger;
    private readonly ICommandHandler<SetApplicationTimeCommand, ApplicationTime> _handler;

    public SetApplicationTimeLogging(
        ILogger logger,
        ICommandHandler<SetApplicationTimeCommand, ApplicationTime> handler
    )
    {
        _logger = logger;
        _handler = handler;
    }

    public async Task<Result<ApplicationTime>> Handle(SetApplicationTimeCommand command)
    {
        ApplicationTime time = await _handler.Handle(command);
        _logger.Information(
            "{Context}. Set new application time: {Time}",
            nameof(SetApplicationTimeLogging),
            time.ToString()
        );
        return time;
    }
}
