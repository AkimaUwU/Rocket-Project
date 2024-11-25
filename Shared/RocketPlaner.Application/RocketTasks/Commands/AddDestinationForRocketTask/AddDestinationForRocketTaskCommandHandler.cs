using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskCommandHandler
    : ICommandHandler<AddDestinationForRocketTaskCommand, RocketTaskDestination>
{
    private readonly IUsersDataBase _usersDb;
    private readonly ITaskDestinationDatabase _destinationsDb;

    public AddDestinationForRocketTaskCommandHandler(
        IUsersDataBase usersDb,
        ITaskDestinationDatabase destinationsDb
    )
    {
        _usersDb = usersDb;
        _destinationsDb = destinationsDb;
    }

    public async Task<Result<RocketTaskDestination>> Handle(
        AddDestinationForRocketTaskCommand command
    )
    {
        var userDao = await _usersDb.GetUser(command.UserId);
        if (userDao is null)
            return UserErrors.UserNotFound;

        var user = userDao.ToUser();

        var rocketTask = user.Tasks.Find(t => t.Title == command.TaskTitle);
        if (rocketTask.IsError)
            return rocketTask.Error;

        var rocketTaskDestination = RocketTaskDestination.Create(
            Guid.NewGuid(),
            rocketTask,
            command.DestinationChatId
        );
        if (rocketTaskDestination.IsError)
            return rocketTaskDestination.Error;

        rocketTaskDestination = rocketTask.Value.Destinations.Add(rocketTaskDestination.Value);
        if (rocketTaskDestination.IsError)
            return rocketTaskDestination.Error;

        await _destinationsDb.AddDestination(
            rocketTaskDestination.Value.ToDestinationsDao(rocketTask.Value.ToRocketTaskDao(userDao))
        );
        return rocketTaskDestination;
    }
}
