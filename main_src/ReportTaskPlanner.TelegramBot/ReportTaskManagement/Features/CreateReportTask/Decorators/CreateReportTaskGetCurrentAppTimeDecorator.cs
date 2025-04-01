using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask.Decorators;

public sealed class CreateReportTaskGetCurrentAppTimeDecorator(
    CreateReportTaskContext context,
    ICommandHandler<CreateReportTaskCommand, ReportTask> handler,
    ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updateAppTime,
    IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>> getAppTime,
    IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>> getTzOptions
) : ICommandHandler<CreateReportTaskCommand, ReportTask>
{
    private readonly CreateReportTaskContext _context = context;
    private readonly ICommandHandler<CreateReportTaskCommand, ReportTask> _handler = handler;

    private readonly ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> _updateAppTime =
        updateAppTime;

    private readonly IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>> _getAppTime =
        getAppTime;

    private readonly IQueryHandler<
        GetTimeZoneDbOptionsQuery,
        Option<TimeZoneDbOptions>
    > _getTzOptions = getTzOptions;

    public async Task<Result<ReportTask>> Handle(CreateReportTaskCommand command)
    {
        Task<Option<ApplicationTime>> getAppTimeTask = _getAppTime.Handle(
            new GetCurrentAppTimeQuery()
        );
        Task<Option<TimeZoneDbOptions>> getTzOptionsTask = _getTzOptions.Handle(
            new GetTimeZoneDbOptionsQuery()
        );
        await Task.WhenAll(getAppTimeTask, getTzOptionsTask);

        Option<ApplicationTime> currentTime = getAppTimeTask.Result;
        Option<TimeZoneDbOptions> tzOptions = getTzOptionsTask.Result;
        if (!currentTime.HasValue)
            return new Error(
                "Не удалось создать задачу. Возможно текущее время бота не настроено."
            );
        if (!tzOptions.HasValue)
            return new Error(
                "Не удалось создать задачу. Возможно Time Zone Db провайдер не настроен."
            );

        UpdateApplicationTimeCommand updateCommand = new(currentTime.Value, tzOptions.Value);
        Result<ApplicationTime> result = await _updateAppTime.Handle(updateCommand);
        if (result.IsFailure)
            return result.Error;

        _context.CurrentTime = Option<ApplicationTime>.Some(result.Value);
        return await _handler.Handle(command);
    }
}
