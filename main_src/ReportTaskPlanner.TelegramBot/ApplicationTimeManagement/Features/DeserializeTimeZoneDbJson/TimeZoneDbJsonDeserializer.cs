﻿using System.Text.Json;
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

        using JsonDocument document = JsonDocument.Parse(_json);
        if (!document.RootElement.TryGetProperty("zones", out JsonElement zonesArrayElement))
            return new Error($"Некорректный ответ от Time Zone Db. Ответ: {_json}");

        ApplicationTime[] times = new ApplicationTime[zonesArrayElement.GetArrayLength()];
        int lastIndex = 0;

        foreach (var element in zonesArrayElement.EnumerateArray())
        {
            JsonElement timeZoneElement = element.GetProperty("zoneName");
            JsonElement timeStampElement = element.GetProperty("timestamp");
            string zoneName = timeZoneElement.GetString()!;
            long timeStamp = timeStampElement.GetInt64();
            DateTime dateTime = timeStamp.FromUnixTime();
            ApplicationTime time = new(zoneName, zoneName, timeStamp, dateTime);
            times[lastIndex] = time;
            lastIndex++;
        }
        return times;
    }
}
