using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ReportTaskPlanner.DependencyInjection;

public static class LoggerInjection
{
    public static void InjectLogger(this IServiceCollection services)
    {
        ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        services.AddSingleton<ILogger>(logger);
    }
}
