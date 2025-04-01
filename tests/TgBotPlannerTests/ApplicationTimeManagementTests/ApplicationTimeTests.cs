using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Serilog;

namespace TgBotPlannerTests.ApplicationTimeManagementTests;

public sealed class ApplicationTimeTests
{
    private readonly IServiceScopeFactory _factory;

    public ApplicationTimeTests()
    {
        IServiceCollection services = new ServiceCollection();
        ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        services.AddSingleton(logger);
        services.InjectAllServices();
        IServiceProvider provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Create_Get_Update_Delete_Tests()
    {
        const string token = "N75LSH9ABRHP";
        using IServiceScope scope = _factory.CreateScope();
        BotPlannerTimeManagementApi api =
            scope.ServiceProvider.GetRequiredService<BotPlannerTimeManagementApi>();
        Result<TimeZoneDbOptions> options = await api.SetTimeZoneDbOptions(token);
        Assert.True(options.IsSuccess);
        Result<ApplicationTime[]> times = await api.ListTimeZones();
        Assert.True(times.IsSuccess);
        ApplicationTime? krsk = times.Value.FirstOrDefault(t => t.DisplayName == "Красноярск");
        Assert.NotNull(krsk);
        Result<ApplicationTime> saving = await api.SaveTime(krsk);
        Assert.True(saving.IsSuccess);
        Assert.Equal(saving.Value.DisplayName, krsk.DisplayName);
        Option<ApplicationTime> current = await api.GetCurrentAppTime();
        Assert.True(current.HasValue);
        Assert.Equal(krsk.DisplayName, current.Value.DisplayName);
        Option<ApplicationTime> updated = await api.GetUpdatedApplicationTime();
        Assert.True(updated.HasValue);
        Assert.Equal(updated.Value.DisplayName, krsk.DisplayName);
        Result<bool> deletion = await api.DeleteAppTime(krsk.ZoneName);
        Assert.True(deletion.IsSuccess);
        Result<bool> deleteToken = await api.DeleteTimeZoneDbToken(token);
        Assert.True(deleteToken.IsSuccess);
    }
}
