using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Serilog;

namespace TgBotPlannerTests;

public sealed class CreateReportTaskTests
{
    private readonly IServiceScopeFactory _factory;

    public CreateReportTaskTests()
    {
        IServiceCollection services = new ServiceCollection();
        Serilog.ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        services.AddSingleton(logger);
        services.InjectAllServices();
        IServiceProvider provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Create_ReportTask_With_Relative_DateTime_Yesterday()
    {
        bool noExceptions = true;
        string input = "завтра в 18:36 назначить генеральную уборку.";
        IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        Random random = new();
        CreateReportTaskCommand command = new CreateReportTaskCommand(input, random.Next(1, 1000));
        try
        {
            ICommandHandler<CreateReportTaskCommand, ReportTask> handler =
                provider.GetRequiredService<ICommandHandler<CreateReportTaskCommand, ReportTask>>();
            Result<ReportTask> result = await handler.Handle(command);
            Assert.True(result.IsSuccess);
        }
        catch
        {
            noExceptions = false;
        }

        scope.Dispose();
        Assert.True(noExceptions);
    }

    [Fact]
    public async Task Create_ReportTask_MultiThread()
    {
        bool noExceptions = true;
        string input = "завтра в 18:36 назначить генеральную уборку.";
        IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        Random random = new();
        CreateReportTaskCommand command1 = new CreateReportTaskCommand(input, random.Next(1, 1000));
        CreateReportTaskCommand command2 = new CreateReportTaskCommand(input, random.Next(1, 1000));
        try
        {
            Task<Result<ReportTask>> task1 = Task.Run(async () =>
            {
                ICommandHandler<CreateReportTaskCommand, ReportTask> handler =
                    provider.GetRequiredService<
                        ICommandHandler<CreateReportTaskCommand, ReportTask>
                    >();
                Result<ReportTask> result = await handler.Handle(command1);
                return result;
            });
            Task<Result<ReportTask>> task2 = Task.Run(async () =>
            {
                ICommandHandler<CreateReportTaskCommand, ReportTask> handler =
                    provider.GetRequiredService<
                        ICommandHandler<CreateReportTaskCommand, ReportTask>
                    >();
                Result<ReportTask> result = await handler.Handle(command2);
                return result;
            });
            Result<ReportTask>[] results = await Task.WhenAll(task1, task2);
            foreach (Result<ReportTask> result in results)
            {
                Assert.True(result.IsSuccess);
            }
        }
        catch
        {
            noExceptions = false;
        }

        scope.Dispose();
        Assert.True(noExceptions);
    }
}
