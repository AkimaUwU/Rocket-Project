using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public class AddTaskForUsersCommandHandler : ICommandHandler<AddTaskForUsersCommand, RocketTask>
{
    private readonly DomainEventDispatcher _dispatcher;
    private readonly IUsersDataBase _users;

    public AddTaskForUsersCommandHandler(DomainEventDispatcher dispatcher, IUsersDataBase users)
    {
        _dispatcher = dispatcher;
        _users = users;
    }

    public async Task<Result<RocketTask>> Handle(AddTaskForUsersCommand command)
    {
        var telegramId = UserTelegramId.Create(command.UserTelegramId);
        if (telegramId.IsError)
            return telegramId.Error;

        var user = await _users.GetUser(telegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var title = RocketTaskTitle.Create(command.Title);
        var message = RocketTaskMessage.Create(command.Message);
        var type = RocketTaskType.Create(command.Type);
        var fireDate = RocketTaskFireDate.Create(command.FireDate);

        if (title.IsError)
            return title.Error;

        if (message.IsError)
            return message.Error;

        if (type.IsError)
            return type.Error;

        if (fireDate.IsError)
            return fireDate.Error;

        var createdTask = user.RegisterRocketTask(title, message, type, fireDate);

        if (createdTask.IsError)
            return createdTask;

        await _dispatcher.Dispatch(user.GetDomainEvents());
        return createdTask;
    }
}
