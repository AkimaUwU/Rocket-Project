using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;

public sealed class ListTimeZonesTimeZoneDbRequester
    : IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>
{
    private readonly ListTimeZonesSharedContext _context;
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _handler;

    public ListTimeZonesTimeZoneDbRequester(
        IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> handler,
        ListTimeZonesSharedContext context
    ) => (_handler, _context) = (handler, context);

    public async Task<ApplicationTime[]> Handle(ListTimeZonesQuery query)
    {
        if (!_context.Options.HasValue)
            return [];

        TimeZoneDbOptions options = _context.Options.Value;
        using HttpClient client = new HttpClient();
        string url = MakeRequestUrl(options);
        HttpResponseMessage responseMessage = await client.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode)
        {
            string content = await responseMessage.Content.ReadAsStringAsync();
            _context.Error = Option<Error>.Some(
                new Error($"Time Zone Db Provider запрос выдал ошибку. Ответ: {content}")
            );
            return [];
        }

        _context.TimeZoneDbResponseJson = Option<string>.Some(
            await responseMessage.Content.ReadAsStringAsync()
        );
        return await _handler.Handle(query);
    }

    private string MakeRequestUrl(TimeZoneDbOptions options) =>
        $"https://api.timezonedb.com/v2.1/list-time-zone&key={options.Token}&format=json&country=RU";
}
