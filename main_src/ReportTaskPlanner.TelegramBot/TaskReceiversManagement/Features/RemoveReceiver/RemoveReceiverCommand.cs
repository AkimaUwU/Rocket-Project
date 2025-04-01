using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver;

public sealed record RemoveReceiverCommand(long Id) : ICommand<bool>;

public sealed class RemoveReceiverCommandHandler(ITaskReceiverRepository repository)
    : ICommandHandler<RemoveReceiverCommand, bool>
{
    private readonly ITaskReceiverRepository _repository = repository;

    public async Task<Result<bool>> Handle(RemoveReceiverCommand command)
    {
        Result deletion = await _repository.Remove(command.Id);
        return deletion.IsSuccess ? true : deletion.Error;
    }
}
