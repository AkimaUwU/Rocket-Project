using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public sealed class AddTaskForUserEventHandler : IDomainEventHandler<UserAddedTask>
{
    private readonly ITaskDataBase _tasks;

    public AddTaskForUserEventHandler(ITaskDataBase tasks) => _tasks = tasks;

    public async Task Handle(UserAddedTask domainEvent) => await _tasks.AddTask(domainEvent.Task);
}
