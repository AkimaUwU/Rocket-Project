using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed record AddDestinationForRocketTaskCommand(
    long? UserTelegramId,
    string? Title,
    long? ChatId
) : ICommand<RocketTaskDestination>;
