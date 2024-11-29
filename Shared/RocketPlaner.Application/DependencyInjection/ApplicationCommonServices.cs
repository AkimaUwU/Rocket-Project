using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;

namespace RocketPlaner.Application.DependencyInjection;

public static class ApplicationCommonServices
{
    public static IServiceCollection AddApplicationCommonServices(this IServiceCollection services)
    {
        services = services
            .AddScoped<ICommandDispatcher, CommandDispatcher>()
            .AddScoped<IQueryDispatcher, QueryDispatcher>()
            .AddScoped<DomainEventDispatcher>();
        return services;
    }
}
