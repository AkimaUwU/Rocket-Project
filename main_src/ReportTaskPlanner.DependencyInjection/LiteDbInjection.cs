using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.LiteDb;
using ReportTaskPlanner.LiteDb.Configurations;
using ReportTaskPlanner.LiteDb.ReportTaskManagement;
using ReportTaskPlanner.UseCases.CreateReportTaskUseCase;

namespace ReportTaskPlanner.DependencyInjection;

public static class LiteDbInjection
{
    public static void InjectLiteDb(this IServiceCollection services)
    {
        LiteDbOptions options = new LiteDbOptions("Filename=database.db;Connection=shared");
        services.AddSingleton(options);

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.InjectQueries(assemblies);
        InjectClassMaps(assemblies);
    }

    private static void InjectQueries(this IServiceCollection services, Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<Type> types = assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<DbQueryAttribute>() != null);
            if (!types.Any())
                continue;
            foreach (var type in types)
            {
                DbQueryAttribute attribute = type.GetCustomAttribute<DbQueryAttribute>()!;
                Type baseType = type.BaseType;
                services.AddTransient(baseType, type);
            }
        }
    }

    private static void InjectClassMaps(Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<MethodInfo> methods = assembly
                .GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => m.GetCustomAttribute<DbObjectMappingAttribute>() != null);
            foreach (var method in methods)
                method.Invoke(null, null);
        }
    }
}
