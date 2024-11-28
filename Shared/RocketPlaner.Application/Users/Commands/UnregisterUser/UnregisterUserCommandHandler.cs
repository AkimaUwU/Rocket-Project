using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UnregisterUser;

public sealed class UnregisterUserCommandHandler : ICommandHandler<UnregisterUserCommand, User>
{
    private readonly IUsersDataBase _users;

    public UnregisterUserCommandHandler(IUsersDataBase users)
    {
        _users = users;
    }

    public async Task<Result<User>> Handle(UnregisterUserCommand command)
    {
        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        if (userTelegramId.IsError)
            return userTelegramId.Error;

        var user = await _users.GetUser(userTelegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        await _users.RemoveUser(user);
        return user;
    }
}
