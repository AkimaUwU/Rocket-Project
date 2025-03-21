using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver;

public sealed record RemoveReceiverCommand(long Id) : ICommand<long>;

public sealed class RemoveReceiverCommandHandler(TaskReceiverRepository repository)
    : ICommandHandler<RemoveReceiverCommand, long>
{
    private readonly TaskReceiverRepository _repository = repository;

    public async Task<Result<long>> Handle(RemoveReceiverCommand command)
    {
        bool isDeleted = await _repository.Remove(command.Id);
        return isDeleted ? command.Id : new Error("Чат не используется ботом.");
    }
}
