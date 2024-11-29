using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    DomainEventDispatcher dispatcher,
    ICommandValidator<RegisterUserCommand, User> validator
) : ICommandHandler<RegisterUserCommand, User>
{
    public async Task<Result<User>> Handle(RegisterUserCommand command)
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var telegramId = UserTelegramId.Create(command.TelegramId);
        var user = new User(telegramId);
        await dispatcher.Dispatch(user.GetDomainEvents());
        return user;
    }
}
