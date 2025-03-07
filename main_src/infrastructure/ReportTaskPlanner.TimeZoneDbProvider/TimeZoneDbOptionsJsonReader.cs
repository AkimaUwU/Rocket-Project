using System.Text.Json;
using ReportTaskPlanner.Main.TimeManagement;

namespace ReportTaskPlanner.TimeZoneDbProvider;

internal sealed class TimeZoneDbOptionsJsonReader
{
    private readonly string _json;

    public TimeZoneDbOptionsJsonReader(string json) => _json = json;

    public IEnumerable<PlannerTime> ConvertToPlannerTimeEnumerable()
    {
        using JsonDocument document = JsonDocument.Parse(_json);
        bool canRead = document.RootElement.TryGetProperty("zones", out JsonElement zonesElement);
        if (!canRead)
            throw new ApplicationException("Невозможно прочитать ответ Time Zone Db");
        
        int arrayLength = zonesElement.GetArrayLength();
        PlannerTime[] array = new PlannerTime[arrayLength];
        int lastIndex = 0;
        foreach (var element in zonesElement.EnumerateArray())
        {
            JsonElement timeZoneElement = element.GetProperty("zoneName");
            JsonElement timeStampElement = element.GetProperty("timestamp");
            string zoneName = timeZoneElement.GetString()!;
            ulong timeStamp = timeStampElement.GetUInt64();
            string displayName = TimeZoneDbZoneNamesDictionary.FromTimeZonesDbDictionary(zoneName);
            PlannerTime plannerTime = new PlannerTime(zoneName, displayName, timeStamp);
            array[lastIndex] = plannerTime;
            lastIndex++;
        }
        return array;
    }
}

internal static class TimeZoneDbZoneNamesDictionary
{
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

    public static string FromTimeZonesDbDictionary(string zoneNameKey)
    {
        string zoneName = _zoneNames[zoneNameKey];
        return zoneName;
    }
}