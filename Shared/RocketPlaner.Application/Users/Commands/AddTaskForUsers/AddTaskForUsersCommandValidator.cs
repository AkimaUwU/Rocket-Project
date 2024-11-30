using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public sealed class AddTaskForUsersCommandValidator
    : BaseValidator,
        ICommandValidator<AddTaskForUsersCommand, RocketTask>
{
    public async Task<bool> IsCommandValidAsync(AddTaskForUsersCommand command)
    {
        AddErrorFromResult(UserTelegramId.Create(command.UserTelegramId));
        AddErrorFromResult(RocketTaskTitle.Create(command.Title));
        AddErrorFromResult(RocketTaskType.Create(command.Type));
        AddErrorFromResult(RocketTaskMessage.Create(command.Message));
        AddErrorFromResult(RocketTaskFireDate.Create(command.FireDate));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
