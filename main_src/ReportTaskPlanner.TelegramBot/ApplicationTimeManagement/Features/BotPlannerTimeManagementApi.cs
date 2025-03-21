using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;

public sealed class BotPlannerTimeManagementApi
{
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _listTimeZones;
    private readonly IQueryHandler<
        GetCurrentAppTimeQuery,
        Option<ApplicationTime>
    > _getCurrentAppTime;
    private readonly IQueryHandler<
        GetTimeZoneDbOptionsQuery,
        Option<TimeZoneDbOptions>
    > _getTimeZoneDbOptions;
    private readonly ICommandHandler<SetApplicationTimeCommand, ApplicationTime> _saveTime;
    private readonly ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> _updateTime;
    private readonly ICommandHandler<
        SetTimeZoneDbOptionsCommand,
        TimeZoneDbOptions
    > _setTimeZoneDbOptions;

    public BotPlannerTimeManagementApi(
        IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> listTimeZones,
        IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>> getCurrentAppTime,
        IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>> getTimeZoneDbOptions,
        ICommandHandler<SetApplicationTimeCommand, ApplicationTime> saveTime,
        ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updateTime,
        ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions> setTimeZoneDbOptions
    )
    {
        _listTimeZones = listTimeZones;
        _getCurrentAppTime = getCurrentAppTime;
        _getTimeZoneDbOptions = getTimeZoneDbOptions;
        _saveTime = saveTime;
        _updateTime = updateTime;
        _setTimeZoneDbOptions = setTimeZoneDbOptions;
    }

    public async Task<ApplicationTime[]> ListTimeZones() =>
        await _listTimeZones.Handle(new ListTimeZonesQuery());

    public async Task<Option<ApplicationTime>> GetCurrentAppTime() =>
        await _getCurrentAppTime.Handle(new GetCurrentAppTimeQuery());

    public async Task<Option<TimeZoneDbOptions>> GetTimeZoneDbOptions() =>
        await _getTimeZoneDbOptions.Handle(new GetTimeZoneDbOptionsQuery());

    public async Task<Result<ApplicationTime>> SaveTime(ApplicationTime time) =>
        await _saveTime.Handle(new SetApplicationTimeCommand(time));

    public async Task<Result<ApplicationTime>> UpdateTime(
        TimeZoneDbOptions options,
        ApplicationTime time
    ) => await _updateTime.Handle(new UpdateApplicationTimeCommand(time, options));

    public async Task<Result<TimeZoneDbOptions>> SetTimeZoneDbOptions(string Token) =>
        await _setTimeZoneDbOptions.Handle(new SetTimeZoneDbOptionsCommand(Token));
}
