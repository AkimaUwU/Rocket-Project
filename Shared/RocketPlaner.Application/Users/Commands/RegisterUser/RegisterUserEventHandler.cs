using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserEventHandler(IUsersDataBase users)
    : IDomainEventHandler<UserCreated>
{
    public async Task Handle(UserCreated domainEvent) => await users.AddUser(domainEvent.User);
}
