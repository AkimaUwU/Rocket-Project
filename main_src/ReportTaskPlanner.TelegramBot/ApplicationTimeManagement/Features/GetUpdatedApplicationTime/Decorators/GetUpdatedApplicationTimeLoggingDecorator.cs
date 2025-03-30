using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime.Decorators;

public sealed class GetUpdatedApplicationTimeLoggingDecorator(
    IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> handler,
    Serilog.ILogger logger
) : IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
{
    private readonly IQueryHandler<
        GetUpdatedApplicationTimeQuery,
        Option<ApplicationTime>
    > _handler = handler;

    private readonly Serilog.ILogger _logger = logger;

    public async Task<Option<ApplicationTime>> Handle(GetUpdatedApplicationTimeQuery query)
    {
        _logger.Information("Performing request: {Name}.", nameof(GetUpdatedApplicationTimeQuery));
        Option<ApplicationTime> time = await _handler.Handle(query);
        return time.ToLoggerChunk(_logger)
            .LogOnNoValue(logger =>
                logger.Error(
                    "{Name} finished. Application time was not set.",
                    nameof(GetUpdatedApplicationTimeQuery)
                )
            )
            .LogOnHasValue(logger =>
                logger.Information(
                    "{Name} finished. Application time result: Zone name: {Prop1} Time stamp: {Prop2} Date string: {Prop3} DateTime {Prop4} DisplayName {Prop5}",
                    nameof(GetUpdatedApplicationTimeQuery),
                    time.Value.ZoneName,
                    time.Value.TimeStamp,
                    time.Value.DateString,
                    time.Value.DateTime,
                    time.Value.DisplayName
                )
            )
            .BackToOption();
    }
}
