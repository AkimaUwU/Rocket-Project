using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.DependencyInjection;

namespace ReportTaskPlanner.Integrationa.Tests;

public class BaseTest
{
    private readonly IServiceProvider _serviceProvider;

    public BaseTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.InjectServices();
        _serviceProvider = services.BuildServiceProvider();
    }

    public T GetService<T>()
    {
        return (T)_serviceProvider.GetRequiredService(typeof(T));
    }
}
