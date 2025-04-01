using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Service.Facade;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Service.Facade.Decorators;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule;

[Injection]
public static class TimeRecognitionDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddSingleton<ITimeRecognitionFacade>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            TimeRecognitionFacade facade = new();
            TimeRecognitionFacadeLoggingDecorator logging = new(facade, logger);
            return logging;
        });
    }
}
