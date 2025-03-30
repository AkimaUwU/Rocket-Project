using System.Reflection;

namespace ReportTaskPlanner.TelegramBot.Shared.Extensions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InjectionAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class InjectionMethod : Attribute;

public static class DependencyInjectionExtensions
{
    public static void InjectAllServices(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Array.ForEach(assemblies, assembly =>  assembly.TryAddServicesFromAssembly(services));
    }
    
    private static void TryAddServicesFromAssembly(this Assembly assembly, IServiceCollection services) => 
        assembly
        .GetInjectionClasses()
        .GetInjectionMethods()
        .InvokeInjectionMethods(services);

    private static IEnumerable<Type> GetInjectionClasses(this Assembly assembly) =>
        assembly.GetTypes().Where(t => t.GetCustomAttribute<InjectionAttribute>() != null);

    private static IEnumerable<MethodInfo> GetInjectionMethods(this IEnumerable<Type> types) =>
        types.SelectMany(t =>
            t.GetMethods().Where(m => m.GetCustomAttribute<InjectionMethod>() != null)
        );

    private static void InvokeInjectionMethods(
        this IEnumerable<MethodInfo> methods,
        IServiceCollection services
    )
    {
        foreach (MethodInfo method in methods)
            method.Invoke(null, [services]);
    }
}
