using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UnregisterUser;

public sealed class UnregisterUserCommandHandler(
    IUsersDataBase users,
    ICommandValidator<UnregisterUserCommand, User> validator
) : ICommandHandler<UnregisterUserCommand, User>
{
    public async Task<Result<User>> Handle(UnregisterUserCommand command)
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        var user = await users.GetUser(userTelegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        await users.RemoveUser(user);
        return user;
    }
}
