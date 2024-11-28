using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;

namespace RocketPlaner.Core.models.RocketTasks.Events;

public record RocketTaskDestinationRemoved(RocketTaskDestination Destination) : IDomainEvent;
