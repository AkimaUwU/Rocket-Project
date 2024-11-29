using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public sealed class AddTaskForUserEventHandler : IDomainEventHandler<UserAddedTask>
{
    private readonly IUsersDataBase _users;

    public AddTaskForUserEventHandler(IUsersDataBase users)
    {
        _users = users;
    }

    public async Task Handle(UserAddedTask domainEvent) =>
        await _users.UpdateUser(domainEvent.User);
}
