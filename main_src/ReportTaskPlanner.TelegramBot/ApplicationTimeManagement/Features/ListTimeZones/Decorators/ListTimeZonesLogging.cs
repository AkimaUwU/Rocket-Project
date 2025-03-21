using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;

public sealed class ListTimeZonesLogging : IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>
{
    private readonly ILogger _logger;
    private readonly ListTimeZonesSharedContext _context;
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _handler;

    public ListTimeZonesLogging(
        IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> handler,
        ListTimeZonesSharedContext context,
        ILogger logger
    ) => (_handler, _logger, _context) = (handler, logger, context);

    public async Task<ApplicationTime[]> Handle(ListTimeZonesQuery query)
    {
        ApplicationTime[] result = await _handler.Handle(query);
        if (_context.Error.HasValue)
            _logger.LogError(_context.Error.Value, nameof(ListTimeZonesQuery));
        if (_context.DeserializedTimeZones.HasValue)
        {
            _logger.Information("{Context}", nameof(ListTimeZonesQuery));
            foreach (var item in _context.DeserializedTimeZones.Value)
            {
                _logger.Information(
                    "Time zone: {TZ}, Display name: {DN}, Date Time: {DT}, Unix Time: {UT}",
                    item.ZoneName,
                    item.DisplayName,
                    item.DateTime,
                    item.TimeStamp
                );
            }
        }
        return result;
    }
}
