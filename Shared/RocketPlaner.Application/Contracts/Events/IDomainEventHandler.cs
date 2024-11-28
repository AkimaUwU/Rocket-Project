using RocketPlaner.Core.Abstractions;

namespace RocketPlaner.Application.Contracts.Events;

public interface IDomainEventHandler<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent);
}
