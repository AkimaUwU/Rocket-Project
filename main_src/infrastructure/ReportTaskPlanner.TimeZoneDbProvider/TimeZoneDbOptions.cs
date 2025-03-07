using System.Text.Json;

namespace ReportTaskPlanner.TimeZoneDbProvider;

public sealed class TimeZoneDbOptions
{
    public string Token { get; }
    public TimeZoneDbOptions(string token) => Token = token;
}

public static class TimeZoneDbOptionsManager
{
    public static TimeZoneDbOptions CreateNewTimeZoneDbOptionsFile(string token, string filePath)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Time Zone Db токен был пустым");
        
        TimeZoneDbOptions options = new TimeZoneDbOptions(token);
        string serializedOptions = JsonSerializer.Serialize(options);
        File.WriteAllText(filePath, serializedOptions);
        return options;
    }

    public static TimeZoneDbOptions UpdateTimeZoneDbOptionsFile(string token, string filePath)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Time Zone Db токен был пустым");
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу конфигурации тайм зоны был пустым");
        
        TimeZoneDbOptions options = GetCurrentTimeZoneDbOptions(filePath);
        File.Delete(filePath);
        
        TimeZoneDbOptions newOptions = CreateNewTimeZoneDbOptionsFile(options.Token, filePath);
        return newOptions;
    }

    public static TimeZoneDbOptions GetCurrentTimeZoneDbOptions(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу конфигурации тайм зоны был пустым");
        
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(filePath));
        bool canGetValue = document.RootElement.TryGetProperty("Token", out JsonElement value);
        if (!canGetValue)
            throw new ArgumentException("Конфигурационный файл настроек тайм зоны некорректный");
        
        string? token = value.GetString();
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException(message: "Значение токена Time Zone Db было пустым");

        return new TimeZoneDbOptions(token);
    }
}