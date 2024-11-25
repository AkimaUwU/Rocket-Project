using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUsersDataBase users)
    : ICommandHandler<RegisterUserCommand, User>
{
    public async Task<Result<User>> Handle(RegisterUserCommand command)
    {
        Result<User> user = User.Create(Guid.NewGuid(), command.TelegramId);
        if (user.IsError)
            return user.Error;

        await users.AddUser(user.Value.ToUsersDao());
        return user;
    }
}
