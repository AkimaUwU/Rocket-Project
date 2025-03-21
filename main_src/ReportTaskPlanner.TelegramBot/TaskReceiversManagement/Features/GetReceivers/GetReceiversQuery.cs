using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.GetReceivers;

public sealed record GetReceiversQuery : IQuery<Option<IEnumerable<TaskReceiver>>>;

public sealed class GetReceiversQueryHandler(TaskReceiverRepository repostiroy)
    : IQueryHandler<GetReceiversQuery, Option<IEnumerable<TaskReceiver>>>
{
    private readonly TaskReceiverRepository _repository = repostiroy;

    public async Task<Option<IEnumerable<TaskReceiver>>> Handle(GetReceiversQuery query)
    {
        IEnumerable<TaskReceiver> receivers = await _repository.GetAll();
        return receivers.Any()
            ? Option<IEnumerable<TaskReceiver>>.Some(receivers)
            : Option<IEnumerable<TaskReceiver>>.None();
    }
}
