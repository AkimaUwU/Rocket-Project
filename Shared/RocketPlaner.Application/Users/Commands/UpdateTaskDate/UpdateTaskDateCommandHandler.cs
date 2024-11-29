using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public class UpdateTaskDateCommandHandler(
    DomainEventDispatcher dispatcher,
    IUsersDataBase users,
    ICommandValidator<UpdateTaskDateCommand, RocketTask> validator
) : ICommandHandler<UpdateTaskDateCommand, RocketTask>
{
    public async Task<Result<RocketTask>> Handle(UpdateTaskDateCommand command)
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var telegramId = UserTelegramId.Create(command.TelegramId);
        var user = await users.GetUser(telegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var title = RocketTaskTitle.Create(command.Title);
        var task = user.FindRocketTask(t => t.Title == title);
        if (task.IsError)
            return task;

        var fireDate = RocketTaskFireDate.Create(command.NewFireDate);
        task.Value.UpdateNotificationDate(fireDate);
        await dispatcher.Dispatch(task.Value.GetDomainEvents());
        return task;
    }
}
