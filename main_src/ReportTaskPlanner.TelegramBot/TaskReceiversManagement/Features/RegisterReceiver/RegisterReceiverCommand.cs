using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver;

public sealed record RegisterReceiverCommand(long Id) : ICommand<TaskReceiver>;

public sealed class RegisterReceiverCommandHandler(TaskReceiverRepository repository)
    : ICommandHandler<RegisterReceiverCommand, TaskReceiver>
{
    private readonly TaskReceiverRepository _repository = repository;

    public async Task<Result<TaskReceiver>> Handle(RegisterReceiverCommand command)
    {
        TaskReceiver receiver = new(command.Id);
        receiver.EnableReceiver();
        await _repository.Save(receiver);
        return receiver;
    }
}
