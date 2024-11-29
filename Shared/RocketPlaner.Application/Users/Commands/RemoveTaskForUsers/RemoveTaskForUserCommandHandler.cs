using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public class RemoveTaskForUserCommandHandler(
    DomainEventDispatcher dispatcher,
    IUsersDataBase users,
    ICommandValidator<RemoveTaskForUsersCommand, RocketTask> validator
) : ICommandHandler<RemoveTaskForUsersCommand, RocketTask>
{
    public async Task<Result<RocketTask>> Handle(RemoveTaskForUsersCommand command)
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var telegramId = UserTelegramId.Create(command.UserTelegramId);
        var title = RocketTaskTitle.Create(command.Title);

        var user = await users.GetUser(telegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var task = user.FindRocketTask(t => t.Title == title);
        task = user.UnregisterRocketTask(task);
        await dispatcher.Dispatch(user.GetDomainEvents());
        return task;
    }
}
