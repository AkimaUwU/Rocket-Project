using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public sealed class RemoveDestinationFromRocketTaskCommandHandler(
    IUsersDataBase usersDb,
    DomainEventDispatcher dispatcher,
    ICommandValidator<RemoveDestinationFromRocketTaskCommand, RocketTaskDestination> validator
) : ICommandHandler<RemoveDestinationFromRocketTaskCommand, RocketTaskDestination>
{
    public async Task<Result<RocketTaskDestination>> Handle(
        RemoveDestinationFromRocketTaskCommand command
    )
    {
        if (!await validator.IsCommandValidAsync(command))
            return validator.GetLastError();

        var userTelegramId = UserTelegramId.Create(command.UserTelegramId);
        var taskTitle = RocketTaskTitle.Create(command.Title);
        var chatId = DestinationChatId.Create(command.ChatId);
        var user = await usersDb.GetUser(userTelegramId);
        if (user is null)
            return UserErrors.UserNotFound;

        var task = user.FindRocketTask(t => t.Title == taskTitle);
        if (task.IsError)
            return task.Error;

        var destination = task.Value.FindDestination(d => d.ChatId == chatId);
        destination = task.Value.RemoveDestination(destination);

        if (destination.IsError)
            return destination;

        await dispatcher.Dispatch(task.Value.GetDomainEvents());
        return destination;
    }
}
