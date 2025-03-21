using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;

public sealed class ListTimeZonesRusificator : IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>
{
    private readonly ListTimeZonesSharedContext _context;
    private readonly IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> _handler;

    private static readonly Dictionary<string, string> _zoneNames = new Dictionary<string, string>()
    {
        { @"Asia/Anadyr", "Анадырь" },
        { @"Asia/Barnaul", "Барнаул" },
        { @"Asia/Chita", "Чита" },
        { @"Asia/Irkutsk", "Иркутск" },
        { @"Asia/Kamchatka", "Камчатка" },
        { @"Asia/Khandyga", "Хадыга" },
        { @"Asia/Krasnoyarsk", "Красноярск" },
        { @"Asia/Magadan", "Магадан" },
        { @"Asia/Novokuznetsk", "Новокузнецк" },
        { @"Asia/Novosibirsk", "Новосибирск" },
        { @"Asia/Omsk", "Омск" },
        { @"Asia/Sakhalin", "Сахалин" },
        { @"Asia/Srednekolymsk", "Среднеколымск" },
        { @"Asia/Tomsk", "Томск" },
        { @"Asia/Ust-Nera", "Усть-Нера" },
        { @"Asia/Vladivostok", "Владивосток" },
        { @"Asia/Yakutsk", "Якутск" },
        { @"Asia/Yekaterinburg", "Екатиренбург" },
        { @"Europe/Astrakhan", "Астрахань" },
        { @"Europe/Kaliningrad", "Калининград" },
        { @"Europe/Kirov", "Киров" },
        { @"Europe/Moscow", "Москва" },
        { @"Europe/Samara", "Самара" },
        { @"Europe/Saratov", "Саратов" },
        { @"Europe/Ulyanovsk", "Ульяновск" },
        { @"Europe/Volgograd", "Волгоград" },
    };

    public static string TryFormat(string zoneNameKey) =>
        _zoneNames.ContainsKey(zoneNameKey) ? _zoneNames[zoneNameKey] : zoneNameKey;

    public ListTimeZonesRusificator(
        IQueryHandler<ListTimeZonesQuery, ApplicationTime[]> handler,
        ListTimeZonesSharedContext context
    ) => (_handler, _context) = (handler, context);

    public async Task<ApplicationTime[]> Handle(ListTimeZonesQuery query)
    {
        if (!_context.DeserializedTimeZones.HasValue)
            return [];
        for (int i = 0; i < _context.DeserializedTimeZones.Value.Length; i++)
        {
            ApplicationTime time = _context.DeserializedTimeZones.Value[i];
            _context.DeserializedTimeZones.Value[i] = new ApplicationTime(
                time.ZoneName,
                TryFormat(time.ZoneName),
                time.TimeStamp,
                time.DateTime
            );
        }

        return await _handler.Handle(query);
    }
}
