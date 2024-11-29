using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.AddTaskForUsers;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;
using RocketPlaner.Application.Users.Commands.UnregisterUser;
using RocketPlaner.Application.Users.Commands.UpdateTaskDate;
using RocketPlaner.Application.Users.Queries.GetUsersTask;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.Events;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Application.Users.DependencyInjection;

public static class UserApplicationServices
{
    public static IServiceCollection AddUserApplicationServices(this IServiceCollection services)
    {
        services = services
            .AddTransient<
                ICommandHandler<AddTaskForUsersCommand, RocketTask>,
                AddTaskForUsersCommandHandler
            >()
            .AddTransient<
                ICommandValidator<AddTaskForUsersCommand, RocketTask>,
                AddTaskForUsersCommandValidator
            >()
            .AddTransient<IDomainEventHandler<UserAddedTask>, AddTaskForUserEventHandler>()
            .AddTransient<ICommandHandler<RegisterUserCommand, User>, RegisterUserCommandHandler>()
            .AddTransient<
                ICommandValidator<RegisterUserCommand, User>,
                RegisterUserCommandValidator
            >()
            .AddTransient<IDomainEventHandler<UserCreated>, RegisterUserEventHandler>()
            .AddTransient<
                ICommandHandler<RemoveTaskForUsersCommand, RocketTask>,
                RemoveTaskForUserCommandHandler
            >()
            .AddTransient<
                ICommandValidator<RemoveTaskForUsersCommand, RocketTask>,
                RemoveTaskForUsersCommandValidator
            >()
            .AddTransient<IDomainEventHandler<UserRemovedTask>, RemoveTaskForUserEventHandler>()
            .AddTransient<
                ICommandHandler<UnregisterUserCommand, User>,
                UnregisterUserCommandHandler
            >()
            .AddTransient<
                ICommandValidator<UnregisterUserCommand, User>,
                UnregisterUserCommandValidator
            >()
            .AddTransient<
                ICommandHandler<UpdateTaskDateCommand, RocketTask>,
                UpdateTaskDateCommandHandler
            >()
            .AddTransient<
                ICommandValidator<UpdateTaskDateCommand, RocketTask>,
                UpdateTaskDateCommandValidator
            >()
            .AddTransient<
                IDomainEventHandler<RocketTaskFireDateUpdated>,
                UpdateTaskDateEventHandler
            >()
            .AddTransient<
                IQueryHandler<GetUsersTaskQuery, IReadOnlyList<RocketTask>>,
                GetUsersTaskQueryHandler
            >()
            .AddTransient<
                IQueryValidator<GetUsersTaskQuery, IReadOnlyList<RocketTask>>,
                GetUsersTaskQueryValidator
            >();
        return services;
    }
}
