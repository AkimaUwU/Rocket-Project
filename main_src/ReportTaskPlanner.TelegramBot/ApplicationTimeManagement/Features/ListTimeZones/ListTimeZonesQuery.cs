using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones;

public sealed record ListTimeZonesQuery : IQuery<ApplicationTime[]>;

public sealed record ListTimeZonesQueryHandler
    : IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>
{
    private readonly ListTimeZonesSharedContext _context;

    public ListTimeZonesQueryHandler(ListTimeZonesSharedContext context) => _context = context;

    public async Task<ApplicationTime[]> Handle(ListTimeZonesQuery query)
    {
        if (_context.Error.HasValue)
            return [];
        return await Task.FromResult(
            _context.DeserializedTimeZones.HasValue ? _context.DeserializedTimeZones.Value : []
        );
    }
}
