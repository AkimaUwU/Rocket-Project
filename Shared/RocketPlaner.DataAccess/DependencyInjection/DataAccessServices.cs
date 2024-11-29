using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.DataAccess.DatabaseImplementations.TasksDatabase;
using RocketPlaner.DataAccess.DatabaseImplementations.TasksDestinationsDataBase;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

namespace RocketPlaner.DataAccess.DependencyInjection;

public static class DataAccessServices
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services = services
            .AddScoped<IUsersDataBase, UsersDatabase>()
            .AddScoped<ITaskDataBase, TasksDatabase>()
            .AddScoped<ITaskDestinationDatabase, TaskDestinationsDatabase>();
        return services;
    }
}
