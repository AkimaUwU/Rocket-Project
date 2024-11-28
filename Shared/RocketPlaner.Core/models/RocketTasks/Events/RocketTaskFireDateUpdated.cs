using RocketPlaner.Core.Abstractions;

namespace RocketPlaner.Core.models.RocketTasks.Events;

public record RocketTaskFireDateUpdated(RocketTask Task) : IDomainEvent;
