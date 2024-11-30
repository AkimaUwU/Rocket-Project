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
        AddErrorFromResult(UserTelegramId.Create(command.UserTelegramId));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
