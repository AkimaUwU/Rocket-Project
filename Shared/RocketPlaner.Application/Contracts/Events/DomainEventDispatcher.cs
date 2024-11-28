using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Core.Abstractions;

namespace RocketPlaner.Application.Contracts.Events;

public sealed class DomainEventDispatcher(IServiceProvider serviceProvider)
{
    public async Task Dispatch(IReadOnlyList<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var handleMethod = handlerType.GetMethod("Handle");
                if (handleMethod != null)
                {
                    await (Task)handleMethod.Invoke(handler, new[] { domainEvent })!;
                }
            }
        }
    }
}
