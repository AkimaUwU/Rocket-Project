using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeserializeTimeZoneDbJson;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;

public sealed class ListTimeZonesResponseDeserializer
    : IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>
{
    private readonly ListTimeZonesSharedContext _context;
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _handler;

    public ListTimeZonesResponseDeserializer(
        ListTimeZonesSharedContext context,
        IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> handler
    ) => (_context, _handler) = (context, handler);

    public async Task<ApplicationTime[]> Handle(ListTimeZonesQuery query)
    {
        if (!_context.TimeZoneDbResponseJson.HasValue)
        {
            _context.Error = Option<Error>.Some(new Error($"Не получен ответ от Time Zone Db."));
            return [];
        }

        string json = _context.TimeZoneDbResponseJson.Value;
        TimeZoneDbJsonDeserializer deserializer = new(json);
        Result<ApplicationTime[]> deserialized = deserializer.Deserialize();
        if (deserialized.IsFailure)
        {
            _context.Error = Option<Error>.Some(deserialized.Error);
            return [];
        }
        _context.DeserializedTimeZones = Option<ApplicationTime[]>.Some(deserialized);
        return await _handler.Handle(query);
    }
}
