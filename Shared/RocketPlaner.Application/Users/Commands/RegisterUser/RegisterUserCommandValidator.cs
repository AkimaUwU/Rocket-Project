using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator(IUsersDataBase usersDataBase)
    : BaseValidator,
        ICommandValidator<RegisterUserCommand, User>
{
    public async Task<bool> IsCommandValidAsync(RegisterUserCommand command)
    {
        var telegramId = UserTelegramId.Create(command.TelegramId);
        if (telegramId.IsError)
            errors.Add(telegramId.Error);

        if (!await usersDataBase.EnsureTelegramIdIsUnique(telegramId))
            errors.Add(UserErrors.TelegramIdIsNotUnuqie);

        return HasErrors;
    }

    public Error GetLastError() => LastError;
}
