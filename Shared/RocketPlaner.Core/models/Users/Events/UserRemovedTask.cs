using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Core.models.Users.Events;

public record UserRemovedTask(User User, RocketTask Task) : IDomainEvent;
