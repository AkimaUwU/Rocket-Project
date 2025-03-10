using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.DependencyInjection;
using Serilog;

namespace ReportTaskPlanner.Integrationa.Tests;

public class BaseTest
{
    private readonly IServiceProvider _serviceProvider;
    protected readonly ILogger _logger;

    public BaseTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.InjectServices();
        _serviceProvider = services.BuildServiceProvider();
        _logger = _serviceProvider.GetRequiredService<ILogger>();
    }

    public T GetService<T>()
    {
        return (T)_serviceProvider.GetRequiredService(typeof(T));
    }
}
