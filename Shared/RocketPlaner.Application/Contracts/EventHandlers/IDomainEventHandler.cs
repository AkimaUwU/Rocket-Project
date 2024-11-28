using RocketPlaner.Core.Abstractions;

namespace RocketPlaner.Application.Contracts.EventHandlers;

public interface IDomainEventHandler<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent @event);
}
