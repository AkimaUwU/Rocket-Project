using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime.Decorators;

public sealed class UpdateApplicationTimeLogging
    : ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>
{
    private readonly ILogger _logger;
    private readonly ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> _handler;

    public UpdateApplicationTimeLogging(
        ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> handler,
        ILogger logger
    )
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<Result<ApplicationTime>> Handle(UpdateApplicationTimeCommand command)
    {
        Result<ApplicationTime> updating = await _handler.Handle(command);
        if (updating.IsFailure)
            _logger.LogError(updating.Error, nameof(UpdateApplicationTimeCommand));
        ApplicationTime updated = updating.Value;
        _logger.Information(
            "{Context}. Updated application time. Updated time: {Time}",
            nameof(UpdateApplicationTimeCommand),
            updated.ToString()
        );
        return updating;
    }
}
