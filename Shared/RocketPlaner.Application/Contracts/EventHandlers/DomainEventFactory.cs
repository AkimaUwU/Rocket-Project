using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Core.Abstractions;

namespace RocketPlaner.Application.Contracts.EventHandlers;

public sealed class DomainEventFactory(IServiceProvider serviceProvider) : IDomainEventFactory
{
    public IDomainEventHandler RequestHandler(IDomainEvent @event)
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
    }
}
