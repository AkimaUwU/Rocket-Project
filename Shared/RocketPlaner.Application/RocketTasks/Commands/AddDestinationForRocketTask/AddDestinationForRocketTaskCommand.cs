using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskCommand : ICommand<RocketTaskDestination>
{
    public long? UserId { get; set; }
    public string? TaskTitle { get; set; }
    public string? DestinationChatId { get; set; }
}
