using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskCommandHandler
    : ICommandHandler<AddDestinationForRocketTaskCommand, RocketTaskDestination>
{
    private readonly IUsersDataBase _usersDb;
    private readonly DomainEventDispatcher _dispatcher;

    public AddDestinationForRocketTaskCommandHandler(
        IUsersDataBase usersDb,
        DomainEventDispatcher dispatcher
    )
    {
        _usersDb = usersDb;
        _dispatcher = dispatcher;
    }

    public async Task<Result<RocketTaskDestination>> Handle(
        AddDestinationForRocketTaskCommand command
    )
    {
        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        if (userTelegramId.IsError)
            return userTelegramId.Error;

        var taskTitle = RocketTaskTitle.Create(command.Title);
        if (taskTitle.IsError)
            return taskTitle.Error;

        var destinationChatId = DestinationChatId.Create(command.ChatId);
        if (destinationChatId.IsError)
            return destinationChatId.Error;

        var user = await _usersDb.GetUser(userTelegramId);
        if (user is null)
            return userTelegramId.Error;

        var task = user.FindRocketTask(t => t.Title == taskTitle);
        if (task.IsError)
            return task.Error;

        var destination = task.Value.AddDestination(destinationChatId);
        if (destination.IsError)
            return destination;

        await _dispatcher.Dispatch(task.Value.GetDomainEvents());
        return destination;
    }
}
