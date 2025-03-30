using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.Utils;
using Serilog;

namespace TgBotPlannerTests;

public sealed class GetTimeZoneDbTimeTests
{
    private readonly IServiceScopeFactory _factory;

    public GetTimeZoneDbTimeTests()
    {
        IServiceCollection services = new ServiceCollection();
        Serilog.ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        services.AddSingleton(logger);
        services.InjectAllServices();
        IServiceProvider provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task GetUpdatedTimeZoneDbTest()
    {
        IServiceScope scope = _factory.CreateScope();
        bool noExceptions = true;

        try
        {
            long currentTime = DateTime.Now.ToUnixTime();
            IServiceProvider provider = scope.ServiceProvider;
            IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> handler =
                provider.GetRequiredService<
                    IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
                >();
            GetUpdatedApplicationTimeQuery query = new();
            Option<ApplicationTime> time = await handler.Handle(query);
            Assert.True(time.HasValue);
            ApplicationTime timeValue = time.Value;
            long difference = CalculateDifference(timeValue.TimeStamp, currentTime);
            Assert.True((difference < 360));
        }
        catch
        {
            noExceptions = false;
        }

        Assert.True(noExceptions);
        scope.Dispose();
    }

    private static long CalculateDifference(long valueA, long valueB)
    {
        return Math.Abs(valueA - valueB);
    }
}
