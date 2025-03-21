using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver.Decorators;

public sealed class RegisterReceiverValidating(
    TaskReceiverRepository repository,
    ICommandHandler<RegisterReceiverCommand, TaskReceiver> handler
) : ICommandHandler<RegisterReceiverCommand, TaskReceiver>
{
    private readonly TaskReceiverRepository _repository = repository;
    private readonly ICommandHandler<RegisterReceiverCommand, TaskReceiver> _handler = handler;

    public async Task<Result<TaskReceiver>> Handle(RegisterReceiverCommand command)
    {
        long id = command.Id;
        if (await _repository.Contains(id))
            return new Error($"Чат с ID: {id} уже добавлен");
        return await _handler.Handle(command);
    }
}
