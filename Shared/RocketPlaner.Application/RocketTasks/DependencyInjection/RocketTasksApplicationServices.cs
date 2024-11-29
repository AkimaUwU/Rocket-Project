using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;
using RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;
using RocketPlaner.Core.models.RocketTasks.Events;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;

namespace RocketPlaner.Application.RocketTasks.DependencyInjection;

public static class RocketTasksApplicationServices
{
    public static IServiceCollection AddRocketTasksServices(this IServiceCollection services)
    {
        services = services
            .AddTransient<
                ICommandHandler<AddDestinationForRocketTaskCommand, RocketTaskDestination>,
                AddDestinationForRocketTaskCommandHandler
            >()
            .AddTransient<
                ICommandValidator<AddDestinationForRocketTaskCommand, RocketTaskDestination>,
                AddDestinationForRocketTaskCommandValidator
            >()
            .AddTransient<
                IDomainEventHandler<RocketTaskDestinationAddedToTask>,
                AddDestinationForRocketTaskEventHandler
            >()
            .AddTransient<
                ICommandHandler<RemoveDestinationFromRocketTaskCommand, RocketTaskDestination>,
                RemoveDestinationFromRocketTaskCommandHandler
            >()
            .AddTransient<
                ICommandValidator<RemoveDestinationFromRocketTaskCommand, RocketTaskDestination>,
                RemoveDestinationFromRocketTaskCommandValidator
            >()
            .AddTransient<
                IDomainEventHandler<RocketTaskDestinationRemoved>,
                RemoveDestinationFromRocketTaskEventHandler
            >();
        return services;
    }
}
