using System.Text.Json;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeserializeTimeZoneDbJson;

public sealed class TimeZoneDbJsonDeserializer
{
    private readonly string _json;

    public TimeZoneDbJsonDeserializer(string json) => _json = json;

    public Result<ApplicationTime[]> Deserialize()
    {
        if (string.IsNullOrWhiteSpace(_json))
            return new Error("Ответ от Time Zone Db был пустым.");

        JsonDocument document = JsonDocument.Parse(_json);
        if (!document.RootElement.TryGetProperty("zones", out JsonElement zonesArrayElement))
        {
            document.Dispose();
            return new Error($"Некорректный ответ от Time Zone Db. Ответ: {_json}");
        }

        ApplicationTime[] times = new ApplicationTime[zonesArrayElement.GetArrayLength()];
        int lastIndex = 0;

        foreach (JsonElement element in zonesArrayElement.EnumerateArray())
        {
            JsonElement timeZoneElement = element.GetProperty("zoneName");
            JsonElement timeStampElement = element.GetProperty("timestamp");
            JsonElement offsetElement = element.GetProperty("gmtOffset");
            string zoneName = timeZoneElement.GetString()!;
            long offSetValue = offsetElement.GetInt64();
            long timeStamp = timeStampElement.GetInt64();
            string displayName = ListTimeZonesRusificator.TryFormat(zoneName);
            DateTime dateTime = timeStamp.FromUnixTime();
            ApplicationTime time = new(zoneName, displayName, timeStamp, dateTime);
            times[lastIndex] = time;
            lastIndex++;
        }

        document.Dispose();
        return times;
    }
}
