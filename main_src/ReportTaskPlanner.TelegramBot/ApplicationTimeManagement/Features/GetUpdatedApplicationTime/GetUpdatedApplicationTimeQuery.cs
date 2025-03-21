using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;

public sealed record GetUpdatedApplicationTimeQuery : IQuery<Option<ApplicationTime>>;

public sealed class GetUpdatedApplicationTimeQueryHandler(
    ApplicationTimeRepository appTimeRepository,
    TimeZoneDbRepository tzRepository,
    ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updateHandler
) : IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
{
    private readonly ApplicationTimeRepository _appTimeRepository = appTimeRepository;
    private readonly TimeZoneDbRepository _tzRepository = tzRepository;

    private readonly ICommandHandler<
        UpdateApplicationTimeCommand,
        ApplicationTime
    > _updateTimeHandler = updateHandler;

    public async Task<Option<ApplicationTime>> Handle(GetUpdatedApplicationTimeQuery query)
    {
        var getAppTime = _appTimeRepository.Get();
        var getTzOptions = _tzRepository.Get();
        await Task.WhenAll(getAppTime, getTzOptions);
        Result<ApplicationTime> appTime = getAppTime.Result;
        Result<TimeZoneDbOptions> opts = getTzOptions.Result;

        if (appTime.IsFailure)
            return Option<ApplicationTime>.None();

        if (opts.IsFailure)
            return Option<ApplicationTime>.None();

        UpdateApplicationTimeCommand updateCommand = new(appTime, opts);
        Result<ApplicationTime> updated = await _updateTimeHandler.Handle(updateCommand);
        if (updated.IsFailure)
            return Option<ApplicationTime>.None();

        return Option<ApplicationTime>.Some(updated);
    }
}
