using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public class UpdateTaskDateCommandHandler : ICommandHandler<UpdateTaskDateCommand, RocketTask>
{
    private readonly DomainEventDispatcher _dispatcher;
    private readonly IUsersDataBase _users;

    public UpdateTaskDateCommandHandler(DomainEventDispatcher dispatcher, IUsersDataBase users)
    {
        _dispatcher = dispatcher;
        _users = users;
    }

    public async Task<Result<RocketTask>> Handle(UpdateTaskDateCommand command)
    {
        var telegramId = UserTelegramId.Create(command.TelegramId);
        if (telegramId.IsError)
            return telegramId.Error;

        var fireDate = RocketTaskFireDate.Create(command.NewFireDate);
        if (fireDate.IsError)
            return fireDate.Error;

        var user = await _users.GetUser(telegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var title = RocketTaskTitle.Create(command.Title);
        var task = user.FindRocketTask(t => t.Title == title);
        if (task.IsError)
            return task;

        task.Value.UpdateNotificationDate(fireDate);
        await _dispatcher.Dispatch(task.Value.GetDomainEvents());
        return task;
    }
}
