using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UnregisterUser;

public sealed class UnregisterUserCommandValidator
    : BaseValidator,
        ICommandValidator<UnregisterUserCommand, User>
{
    public async Task<bool> IsCommandValidAsync(UnregisterUserCommand command)
    {
        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        if (userTelegramId.IsError)
            errors.Add(userTelegramId.Error);

        return await Task.FromResult(errors.Count == 0);
    }

    public Error GetLastError() => LastError;
}
