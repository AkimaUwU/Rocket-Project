using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public sealed class RemoveTaskForUserEventHandler : IDomainEventHandler<UserRemovedTask>
{
    private readonly ITaskDataBase _tasks;

    public RemoveTaskForUserEventHandler(ITaskDataBase tasks)
    {
        _tasks = tasks;
    }

    public async Task Handle(UserRemovedTask domainEvent) => await _tasks.Remove(domainEvent.Task);
}
