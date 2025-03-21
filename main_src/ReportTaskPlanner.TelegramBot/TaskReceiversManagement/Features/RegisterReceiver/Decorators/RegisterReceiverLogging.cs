using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver.Decorators;

public sealed class RegisterReceiverLogging(
    Serilog.ILogger logger,
    ICommandHandler<RegisterReceiverCommand, TaskReceiver> handler
) : ICommandHandler<RegisterReceiverCommand, TaskReceiver>
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly ICommandHandler<RegisterReceiverCommand, TaskReceiver> _handler = handler;

    public async Task<Result<TaskReceiver>> Handle(RegisterReceiverCommand command)
    {
        _logger.Information("{Context} invoked command.", nameof(RegisterReceiverCommand));
        Result<TaskReceiver> result = await _handler.Handle(command);
        if (result.IsFailure)
            _logger.LogError(result.Error, nameof(RegisterReceiverCommand));
        else
            _logger.Information("{Context} added task receiver with ID: {ID}", result.Value.Id);
        return result;
    }
}
