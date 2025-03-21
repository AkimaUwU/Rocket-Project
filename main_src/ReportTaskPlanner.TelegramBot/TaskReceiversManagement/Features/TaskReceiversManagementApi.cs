using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features;

public sealed class TaskReceiversManagementApi(
    ICommandHandler<RegisterReceiverCommand, TaskReceiver> registerReceiver,
    ICommandHandler<RemoveReceiverCommand, long> removeReceiver
)
{
    private readonly ICommandHandler<RegisterReceiverCommand, TaskReceiver> _registerReceiver =
        registerReceiver;
    private readonly ICommandHandler<RemoveReceiverCommand, long> _removeReceiver = removeReceiver;

    public async Task<Result<TaskReceiver>> RegisterReceiver(long id)
    {
        RegisterReceiverCommand command = new(id);
        return await _registerReceiver.Handle(command);
    }

    public async Task<long> RemoveReceiver(long id)
    {
        RemoveReceiverCommand command = new(id);
        return await _removeReceiver.Handle(command);
    }
}
