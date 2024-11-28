using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public record RemoveDestinationFromRocketTaskCommand(
    long? UserTelegramId,
    string? Title,
    long? ChatId
) : ICommand<RocketTaskDestination>;
