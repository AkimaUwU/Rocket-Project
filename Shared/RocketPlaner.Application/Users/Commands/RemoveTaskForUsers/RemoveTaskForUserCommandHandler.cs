using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public class RemoveTaskForUserCommandHandler
    : ICommandHandler<RemoveTaskForUsersCommand, RocketTask>
{
    private readonly DomainEventDispatcher _dispatcher;
    private readonly IUsersDataBase _users;

    public RemoveTaskForUserCommandHandler(DomainEventDispatcher dispatcher, IUsersDataBase users)
    {
        _dispatcher = dispatcher;
        _users = users;
    }

    public async Task<Result<RocketTask>> Handle(RemoveTaskForUsersCommand command)
    {
        var telegramId = UserTelegramId.Create(command.UserTelegramId);
        if (telegramId.IsError)
            return telegramId.Error;

        var title = RocketTaskTitle.Create(command.Title);
        if (title.IsError)
            return title.Error;

        var user = await _users.GetUser(telegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var task = user.FindRocketTask(t => t.Title == title);
        task = user.UnregisterRocketTask(task);
        await _dispatcher.Dispatch(user.GetDomainEvents());
        return task;
    }
}
