using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver.Decorators;

public sealed class RemoveReceiverLogging(
    Serilog.ILogger logger,
    ICommandHandler<RemoveReceiverCommand, long> handler
) : ICommandHandler<RemoveReceiverCommand, long>
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly ICommandHandler<RemoveReceiverCommand, long> _handler = handler;

    public async Task<Result<long>> Handle(RemoveReceiverCommand command)
    {
        Result<long> result = await _handler.Handle(command);
        if (result.IsFailure)
            _logger.LogError(result.Error, nameof(RemoveReceiverCommand));
        else
            _logger.Information(
                "{Context} task receiver with ID: {ID} removed.",
                nameof(RemoveReceiverCommand),
                result.Value
            );

        return result;
    }
}
