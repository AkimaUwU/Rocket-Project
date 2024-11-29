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
        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        if (userTelegramId.IsError)
            errors.Add(userTelegramId.Error);

        var title = RocketTaskTitle.Create(command.Title);
        if (title.IsError)
            errors.Add(title.Error);

        var type = RocketTaskType.Create(command.Type);
        if (type.IsError)
            errors.Add(type.Error);

        var message = RocketTaskMessage.Create(command.Message);
        if (message.IsError)
            errors.Add(message.Error);

        var date = RocketTaskFireDate.Create(command.FireDate);
        if (date.IsError)
            errors.Add(date.Error);

        return await Task.FromResult(errors.Count == 0);
    }

    public Error GetLastError() => LastError;
}
