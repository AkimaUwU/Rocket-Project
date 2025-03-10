using System.Text.Json;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.TimeZoneDbProvider;

public sealed record TimeZoneDbOptions
{
    public string Token { get; }
    public TimeZoneDbOptions(string token) => Token = token;
}

public delegate Result<TimeZoneDbOptions> GetTimeZoneDbOptionsFromFile(string filePath);
public delegate Result SaveTimeZoneDbOptionsAsFile(string filePath, string token);
public delegate Result<TimeZoneDbOptions> UpdateTimeZoneDbOptions(string filePath, TimeZoneDbOptions options);

public static class TimeZoneDbOptionsActions
{
    public static GetTimeZoneDbOptionsFromFile GetTimeZoneDbOptions = (filePath) =>
    {
        if (!File.Exists(filePath))
            return new Error("Не существует конфигурации Time zone Db Options");
        
        string timeZoneDbOptionsJson = File.ReadAllText(filePath);
        using JsonDocument document = JsonDocument.Parse(timeZoneDbOptionsJson);
        bool canGetValue = document.RootElement.TryGetProperty("Token", out JsonElement value);
        
        if (!canGetValue)
            return new Error("Конфигурационный файл Time Zone Db Options не корректный");
        
        string? token = value.GetString();
        
        if (string.IsNullOrWhiteSpace(token))
            return new Error("Конфигурационный файл Time Zone Db Options не корректный");
        
        TimeZoneDbOptions options = new(token);
        return options;
    };

    public static SaveTimeZoneDbOptionsAsFile SaveTimeZoneDbOptions = (filePath, token) =>
    {
        if (string.IsNullOrWhiteSpace(token))
            return new Error("Токен Time Zone Db Options некорректный.");

        TimeZoneDbOptions options = new(token);
        string serialized = JsonSerializer.Serialize(options);
        
        File.WriteAllText(filePath, serialized);
        return Result.Success();
    };

    public static UpdateTimeZoneDbOptions UpdateTimeZoneDbOptionsFile = (filePath, options) =>
    {
        Result<TimeZoneDbOptions> existingOptions = GetTimeZoneDbOptions(filePath);
        if (!existingOptions.IsSuccess)
            return existingOptions.Error;
        
        File.Delete(filePath);
        Result saveUpdatedOptions = SaveTimeZoneDbOptions(filePath, options.Token);
        
        if (!saveUpdatedOptions.IsSuccess)
        {
            SaveTimeZoneDbOptions(filePath, existingOptions.Value.Token);
            return new Error(
                $"Не удалось сохранить новые настройки конфигурации Time Zone Db. Предыдущая конфигурация восстановлена для корректной работы приложения. Ошибка: {saveUpdatedOptions.Error.Message}");
        }

        return options;
    };
}