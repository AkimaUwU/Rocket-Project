using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Core.models.RocketTasks.Events;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public sealed class RemoveDestinationFromRocketTaskEventHandler(
    ITaskDestinationDatabase destinations
) : IDomainEventHandler<RocketTaskDestinationRemoved>
{
    public async Task Handle(RocketTaskDestinationRemoved domainEvent) =>
        await destinations.RemoveDestination(domainEvent.Destination);
}
