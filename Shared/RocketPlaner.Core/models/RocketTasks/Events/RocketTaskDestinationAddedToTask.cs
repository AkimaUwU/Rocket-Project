using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTaskDestinations;

namespace RocketPlaner.Core.models.RocketTasks.Events;

public record RocketTaskDestinationAddedToTask(RocketTask Task, RocketTaskDestination Destination)
    : IDomainEvent;
