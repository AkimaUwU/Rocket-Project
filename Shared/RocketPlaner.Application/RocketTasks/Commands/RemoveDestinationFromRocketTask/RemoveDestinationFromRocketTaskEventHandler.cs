using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.RocketTasks.Events;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public sealed class RemoveDestinationFromRocketTaskEventHandler
    : IDomainEventHandler<RocketTaskDestinationRemoved>
{
    private readonly ITaskDestinationDatabase _destinations;

    public RemoveDestinationFromRocketTaskEventHandler(ITaskDestinationDatabase destinations) =>
        _destinations = destinations;

    public async Task Handle(RocketTaskDestinationRemoved domainEvent) =>
        await _destinations.RemoveDestination(domainEvent.Destination);
}
