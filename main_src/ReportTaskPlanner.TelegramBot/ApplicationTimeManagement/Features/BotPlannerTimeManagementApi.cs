using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteTimeZoneDbToken;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateTimeZoneDbToken;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;

public sealed class BotPlannerTimeManagementApi(
    IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> listTimeZones,
    IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>> getCurrentAppTime,
    IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>> getTimeZoneDbOptions,
    IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> getUpdatedAppTime,
    ICommandHandler<SetApplicationTimeCommand, ApplicationTime> saveTime,
    ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updateTime,
    ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions> setTimeZoneDbOptions,
    ICommandHandler<DeleteTimeZoneDbTokenCommand, bool> deleteTzTokenHandler,
    ICommandHandler<UpdateTimeZoneDbTokenCommand, bool> updateTzTokenHandler,
    ICommandHandler<DeleteApplicationTimeCommand, bool> deleteAppTimeHandler
)
{
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _listTimeZones =
        listTimeZones;

    private readonly ICommandHandler<DeleteTimeZoneDbTokenCommand, bool> _deleteTzTokenHandler =
        deleteTzTokenHandler;

    private readonly ICommandHandler<UpdateTimeZoneDbTokenCommand, bool> _updateTzTokenHandler =
        updateTzTokenHandler;

    private readonly ICommandHandler<DeleteApplicationTimeCommand, bool> _deleteAppTimeHandler =
        deleteAppTimeHandler;

    private readonly IQueryHandler<
        GetCurrentAppTimeQuery,
        Option<ApplicationTime>
    > _getCurrentAppTime = getCurrentAppTime;

    private readonly IQueryHandler<
        GetTimeZoneDbOptionsQuery,
        Option<TimeZoneDbOptions>
    > _getTimeZoneDbOptions = getTimeZoneDbOptions;

    private readonly ICommandHandler<SetApplicationTimeCommand, ApplicationTime> _saveTime =
        saveTime;

    private readonly ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> _updateTime =
        updateTime;

    private readonly ICommandHandler<
        SetTimeZoneDbOptionsCommand,
        TimeZoneDbOptions
    > _setTimeZoneDbOptions = setTimeZoneDbOptions;

    private readonly IQueryHandler<
        GetUpdatedApplicationTimeQuery,
        Option<ApplicationTime>
    > _getUpdatedAppTime = getUpdatedAppTime;

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

    public async Task<Result<TimeZoneDbOptions>> SetTimeZoneDbOptions(string token) =>
        await _setTimeZoneDbOptions.Handle(new SetTimeZoneDbOptionsCommand(token));

    public async Task<Option<ApplicationTime>> GetUpdatedApplicationTime() =>
        await _getUpdatedAppTime.Handle(new GetUpdatedApplicationTimeQuery());

    public async Task<Result<bool>> DeleteTimeZoneDbToken(string token) =>
        await _deleteTzTokenHandler.Handle(new DeleteTimeZoneDbTokenCommand(token));

    public async Task<Result<bool>> UpdateTimeZoneDbToken(string token) =>
        await _updateTzTokenHandler.Handle(new UpdateTimeZoneDbTokenCommand(token));

    public async Task<Result<bool>> DeleteAppTime(string ZoneName) =>
        await _deleteAppTimeHandler.Handle(new DeleteApplicationTimeCommand(ZoneName));
}
