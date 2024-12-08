using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskCommandHandler(
    IUsersDataBase usersDb,
    DomainEventDispatcher dispatcher,
    ICommandValidator<AddDestinationForRocketTaskCommand, RocketTaskDestination> validator
) : ICommandHandler<AddDestinationForRocketTaskCommand, RocketTaskDestination>
{
    public async Task<Result<RocketTaskDestination>> Handle(
        AddDestinationForRocketTaskCommand command
    )
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        var taskTitle = RocketTaskTitle.Create(command.Title);
        var destinationChatId = DestinationChatId.Create(command.ChatId);
        var user = await usersDb.GetUser(userTelegramId);

        if (user is null)
            return userTelegramId.Error;

        var task = user.FindRocketTask(t => t.Title == taskTitle);
        if (task.IsError)
            return task.Error;

        var destination = task.Value.AddDestination(destinationChatId);
        if (destination.IsError)
            return destination;

        await dispatcher.Dispatch(task.Value.GetDomainEvents());
        return destination;
    }
}
