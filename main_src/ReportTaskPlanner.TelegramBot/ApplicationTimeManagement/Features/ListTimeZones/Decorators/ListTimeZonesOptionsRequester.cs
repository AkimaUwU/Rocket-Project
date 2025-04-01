using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;

public sealed class ListTimeZonesOptionsRequester
    : IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>
{
    private readonly ITimeZoneDbRepository _repository;
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _handler;
    private readonly ListTimeZonesSharedContext _context;

    public ListTimeZonesOptionsRequester(
        IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> handler,
        ListTimeZonesSharedContext context,
        ITimeZoneDbRepository repository
    ) => (_handler, _context, _repository) = (handler, context, repository);

    public async Task<ApplicationTime[]> Handle(ListTimeZonesQuery query)
    {
        Result<TimeZoneDbOptions> options = await _repository.Get();
        if (options.IsFailure)
        {
            _context.Error = Option<Error>.Some(options.Error);
            return [];
        }

        _context.Options = Option<TimeZoneDbOptions>.Some(options);
        return await _handler.Handle(query);
    }
}
