using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserEventHandler : IDomainEventHandler<UserCreated>
{
    private readonly IUsersDataBase _users;

    public RegisterUserEventHandler(IUsersDataBase users) => _users = users;

    public async Task Handle(UserCreated domainEvent) => await _users.AddUser(domainEvent.User);
}
