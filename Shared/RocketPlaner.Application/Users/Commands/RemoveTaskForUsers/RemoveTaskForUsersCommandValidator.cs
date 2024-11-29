using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public sealed class RemoveTaskForUsersCommandValidator
    : BaseValidator,
        ICommandValidator<RemoveTaskForUsersCommand, RocketTask>
{
    public async Task<bool> IsCommandValidAsync(RemoveTaskForUsersCommand command)
    {
        var telegramId = UserTelegramId.Create(command.UserTelegramId);
        if (telegramId.IsError)
            errors.Add(telegramId.Error);

        var title = RocketTaskTitle.Create(command.Title);
        if (title.IsError)
            errors.Add(title.Error);

        return await Task.FromResult(errors.Count == 0);
    }

    public Error GetLastError() => LastError;
}
