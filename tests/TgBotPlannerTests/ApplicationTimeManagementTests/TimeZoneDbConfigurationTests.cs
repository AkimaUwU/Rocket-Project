using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteTimeZoneDbToken;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Serilog;

namespace TgBotPlannerTests.ApplicationTimeManagementTests;

public sealed class TimeZoneDbConfigurationTests
{
    private readonly IServiceScopeFactory _factory;

    public TimeZoneDbConfigurationTests()
    {
        ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(logger);
        services.InjectAllServices();
        IServiceProvider provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Create_And_Delete_Time_Zone_Db_Token()
    {
        string tokenValue = "N75LSH9ABRHP";
        IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions> handler =
            provider.GetRequiredService<
                ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>
            >();
        ICommandHandler<DeleteTimeZoneDbTokenCommand, bool> deleteHandler =
            provider.GetRequiredService<ICommandHandler<DeleteTimeZoneDbTokenCommand, bool>>();
        IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>> getHandler =
            provider.GetRequiredService<
                IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>>
            >();
        SetTimeZoneDbOptionsCommand command = new(tokenValue);
        Result<TimeZoneDbOptions> result = await handler.Handle(command);
        Assert.True(result.IsSuccess);
        Assert.Equal(tokenValue, result.Value.Token);
        DeleteTimeZoneDbTokenCommand deleteCommand = new(result.Value.Token);
        Result<bool> deletion = await deleteHandler.Handle(deleteCommand);
        Assert.True(deletion.IsSuccess);
        Option<TimeZoneDbOptions> opts = await getHandler.Handle(new GetTimeZoneDbOptionsQuery());
        Assert.False(opts.HasValue);
        scope.Dispose();
    }

    [Fact]
    public async Task Create_And_Delete_And_Update_Time_Zone_Db_Token()
    {
        string oldToken = "12323123DASDSADSA";
        string newToken = "22223231DDDSADDSD";
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        BotPlannerTimeManagementApi api =
            provider.GetRequiredService<BotPlannerTimeManagementApi>();
        Result<TimeZoneDbOptions> creation = await api.SetTimeZoneDbOptions(oldToken);
        Assert.True(creation.IsSuccess);
        Result<bool> updating = await api.UpdateTimeZoneDbToken(newToken);
        Assert.True(updating.IsSuccess);
        Option<TimeZoneDbOptions> opts = await api.GetTimeZoneDbOptions();
        Assert.True(opts.HasValue);
        Assert.Equal(newToken, opts.Value.Token);
        Result<bool> deletion = await api.DeleteTimeZoneDbToken(newToken);
        Assert.True(deletion);
    }

    public static async Task<Result<TimeZoneDbOptions>> CreateNew(
        string tokenValue,
        IServiceScopeFactory factory
    )
    {
        using IServiceScope scope = factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions> handler =
            provider.GetRequiredService<
                ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>
            >();
        return await handler.Handle(new SetTimeZoneDbOptionsCommand(tokenValue));
    }

    public static async Task<Result<bool>> Delete(string tokenValue, IServiceScopeFactory factory)
    {
        using IServiceScope scope = factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        ICommandHandler<DeleteTimeZoneDbTokenCommand, bool> handler = provider.GetRequiredService<
            ICommandHandler<DeleteTimeZoneDbTokenCommand, bool>
        >();
        return await handler.Handle(new DeleteTimeZoneDbTokenCommand(tokenValue));
    }

    public static async Task<Option<TimeZoneDbOptions>> Get(IServiceScopeFactory factory)
    {
        using IServiceScope scope = factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>> handler =
            provider.GetRequiredService<
                IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>>
            >();
        return await handler.Handle(new GetTimeZoneDbOptionsQuery());
    }
}
