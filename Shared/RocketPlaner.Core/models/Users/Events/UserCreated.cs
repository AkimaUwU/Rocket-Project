using RocketPlaner.Core.Abstractions;

namespace RocketPlaner.Core.models.Users.Events;

public record UserCreated(User User) : IDomainEvent;
