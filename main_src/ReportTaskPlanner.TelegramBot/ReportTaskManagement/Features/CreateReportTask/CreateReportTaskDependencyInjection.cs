using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask.Decorators;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;

[Injection]
public static class CreateReportTaskDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateReportTaskCommand, ReportTask>>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            ReportTaskRepository repository = p.GetRequiredService<ReportTaskRepository>();
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
            CompositeDateConverter converter = new();
            converter = converter
                .With(new RelativeDateRegexConverter())
                .With(new DayOfWeekRegexConverter())
                .With(new MonthDateRegexConverter());

            CreateReportTaskCommandHandler h1 = new(repository, context, converter);

            CreateReportTaskGetCurrentAppTimeDecorator h2 = new(
                context,
                h1,
                updateAppTime,
                getAppTime,
                getTzOptions
            );
            CreateReportTaskRecognizeTimeDecorator h3 = new(context, h2);
            CreateReportTaskExceptionDecorator h4 = new(h3, logger);
            CreateReportTaskLoggingDecorator h5 = new(logger, h4);
            return h5;
        });
    }
}
