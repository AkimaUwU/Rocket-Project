using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.RocketTasks.Events;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public sealed class UpdateTaskDateEventHandler : IDomainEventHandler<RocketTaskFireDateUpdated>
{
    private readonly ITaskDataBase _tasks;

    public UpdateTaskDateEventHandler(ITaskDataBase tasks) => _tasks = tasks;

    public async Task Handle(RocketTaskFireDateUpdated domainEvent) =>
        await _tasks.Update(domainEvent.Task);
}
