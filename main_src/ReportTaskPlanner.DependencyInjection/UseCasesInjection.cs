using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.UseCases.Abstractions.Handlers;

namespace ReportTaskPlanner.DependencyInjection;

public static class UseCasesInjection
{
    public static void InjectUseCases(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(AbstractValidator<>)))
                .AsSelf()
                .WithTransientLifetime()
        );
        services.AddSingleton<RequestDispatcher>();
    }
}
