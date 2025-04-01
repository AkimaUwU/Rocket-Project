using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask.Decorators;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Service.Facade;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;

[Injection]
public static class CreateReportTaskDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<CreateReportTaskCommand, ReportTask>>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            IReportTaskRepository repository = p.GetRequiredService<IReportTaskRepository>();
            ITimeRecognitionFacade facade = p.GetRequiredService<ITimeRecognitionFacade>();
            CreateReportTaskContext context = new();
            IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>> getAppTime =
                p.GetRequiredService<
                    IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>>
                >();
            IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>> getTzOptions =
                p.GetRequiredService<
                    IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>>
                >();
            ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updateAppTime =
                p.GetRequiredService<
                    ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>
                >();

            CreateReportTaskCommandHandler h1 = new(repository, context, facade);

            CreateReportTaskGetCurrentAppTimeDecorator h2 = new(
                context,
                h1,
                updateAppTime,
                getAppTime,
                getTzOptions
            );
            CreateReportTaskExceptionDecorator h3 = new(h2, logger);
            CreateReportTaskLoggingDecorator h4 = new(logger, h3);
            return h4;
        });
    }
}
