using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskCommand : ICommand<RocketTaskDestination>
{
    public long? UserId { get; init; }
    public string? TaskTitle { get; init; }
    public string? DestinationChatId { get; init; }

    public AddDestinationForRocketTaskCommand(
        long? userId,
        string? taskTitle,
        string? destinationChatId
    )
    {
        UserId = userId;
        TaskTitle = taskTitle;
        DestinationChatId = destinationChatId;
    }
}
