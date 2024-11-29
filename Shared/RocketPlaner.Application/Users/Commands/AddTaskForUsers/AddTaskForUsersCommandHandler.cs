using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public class AddTaskForUsersCommandHandler(
    DomainEventDispatcher dispatcher,
    IUsersDataBase users,
    ICommandValidator<AddTaskForUsersCommand, RocketTask> validator
) : ICommandHandler<AddTaskForUsersCommand, RocketTask>
{
    public async Task<Result<RocketTask>> Handle(AddTaskForUsersCommand command)
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var telegramId = UserTelegramId.Create(command.UserTelegramId);
        var user = await users.GetUser(telegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var title = RocketTaskTitle.Create(command.Title);
        var message = RocketTaskMessage.Create(command.Message);
        var type = RocketTaskType.Create(command.Type);
        var fireDate = RocketTaskFireDate.Create(command.FireDate);
        var createdTask = user.RegisterRocketTask(title, message, type, fireDate);

        if (createdTask.IsError)
            return createdTask;

        await dispatcher.Dispatch(user.GetDomainEvents());
        return createdTask;
    }
}
