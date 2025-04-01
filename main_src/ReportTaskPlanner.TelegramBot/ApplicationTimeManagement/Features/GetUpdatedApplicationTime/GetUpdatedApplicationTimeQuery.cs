using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;

public sealed record GetUpdatedApplicationTimeQuery : IQuery<Option<ApplicationTime>>;

public sealed class GetUpdatedApplicationTimeQueryHandler(
    IApplicationTimeRepository appTimeRepository,
    ITimeZoneDbRepository tzRepository,
    ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updateHandler
) : IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
{
    private readonly IApplicationTimeRepository _appTimeRepository = appTimeRepository;
    private readonly ITimeZoneDbRepository _tzRepository = tzRepository;

    private readonly ICommandHandler<
        UpdateApplicationTimeCommand,
        ApplicationTime
    > _updateTimeHandler = updateHandler;

    public async Task<Option<ApplicationTime>> Handle(GetUpdatedApplicationTimeQuery query)
    {
        Task<Result<ApplicationTime>> getAppTime = _appTimeRepository.Get();
        Task<Result<TimeZoneDbOptions>> getTzOptions = _tzRepository.Get();
        await Task.WhenAll(getAppTime, getTzOptions);
        Result<ApplicationTime> appTime = getAppTime.Result;
        Result<TimeZoneDbOptions> opts = getTzOptions.Result;

        if (appTime.IsFailure || opts.IsFailure)
            return Option<ApplicationTime>.None();

        UpdateApplicationTimeCommand updateCommand = new(appTime, opts);
        Result<ApplicationTime> updated = await _updateTimeHandler.Handle(updateCommand);
        return updated.IsFailure
            ? Option<ApplicationTime>.None()
            : Option<ApplicationTime>.Some(updated);
    }
}
