using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public sealed class RemoveDestinationFromRocketTaskCommandHandler
    : ICommandHandler<RemoveDestinationFromRocketTaskCommand, RocketTaskDestination>
{
    private readonly IUsersDataBase _usersDb;
    private readonly ITaskDestinationDatabase _destDb;

    public RemoveDestinationFromRocketTaskCommandHandler(
        IUsersDataBase usersDb,
        ITaskDestinationDatabase destDb
    )
    {
        _usersDb = usersDb;
        _destDb = destDb;
    }

    public async Task<Result<RocketTaskDestination>> Handle(
        RemoveDestinationFromRocketTaskCommand command
    )
    {
        var userDao = await _usersDb.GetUser(command.TelegramId);
        if (userDao is null)
            return UserErrors.UserNotFound;

        var userModel = userDao.ToUser();
        var task = userModel.Tasks.Find(t => t.Title == command.Title);
        if (task.IsError)
            return task.Error;

        var destination = task.Value.Destinations.Find(d => d.ChatId == command.ChatId);
        if (destination.IsError)
            return RocketTaskDestinationErrors.DestinationNotFound;

        var taskDao = task.Value.ToRocketTaskDao(userDao);
        var destinationDao = destination.Value.ToDestinationsDao(taskDao);
        await _destDb.RemoveDestination(destinationDao);
        return destination;
    }
}
