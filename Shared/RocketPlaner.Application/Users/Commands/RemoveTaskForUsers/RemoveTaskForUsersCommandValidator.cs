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
        AddErrorFromResult(UserTelegramId.Create(command.UserTelegramId));
        AddErrorFromResult(RocketTaskTitle.Create(command.Title));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
