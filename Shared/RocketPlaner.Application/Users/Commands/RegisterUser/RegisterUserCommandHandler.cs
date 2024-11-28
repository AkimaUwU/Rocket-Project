using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, User>
{
    private readonly DomainEventDispatcher _dispatcher;
    private readonly IUsersDataBase _users;

    public RegisterUserCommandHandler(DomainEventDispatcher dispatcher, IUsersDataBase users)
    {
        _dispatcher = dispatcher;
        _users = users;
    }

    public async Task<Result<User>> Handle(RegisterUserCommand command)
    {
        var telegramId = UserTelegramId.Create(command.TelegramId);
        if (telegramId.IsError)
            return telegramId.Error;

        if (!await _users.EnsureTelegramIdIsUnique(telegramId))
            return UserErrors.TelegramIdIsNotUnuqie;

        var user = new User(telegramId);
        await _dispatcher.Dispatch(user.GetDomainEvents());
        return user;
    }
}
