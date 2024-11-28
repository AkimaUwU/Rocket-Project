using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.RocketTasks.Events;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskEventHandler
    : IDomainEventHandler<RocketTaskDestinationAddedToTask>
{
    private readonly ITaskDestinationDatabase _destinations;

    public AddDestinationForRocketTaskEventHandler(ITaskDestinationDatabase destinations) =>
        _destinations = destinations;

    public async Task Handle(RocketTaskDestinationAddedToTask domainEvent) =>
        await _destinations.AddDestination(domainEvent.Destination);
}
