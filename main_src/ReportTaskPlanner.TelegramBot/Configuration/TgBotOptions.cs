using System.Text.Json;

namespace ReportTaskPlanner.TelegramBot.Configuration;

public sealed record TgBotOptions
{
    public string Token { get; }

    public TgBotOptions(string token)
    {
        Token = token;
    }
}

public static class TgBotOptionsResolver
{
    public static void ResolveTgOptions()
    {
        Console.WriteLine("Вас приветствует настройка телеграм-бот сервиса планировщика задач.");
        Console.WriteLine("Для того, чтобы бот работал, его нужно создать в телеграм чате - @BotFather.");
        Console.WriteLine("Пожалуйста, перейдите в чат создателя ботов и выполните процедуру создания бота.");
        Console.WriteLine("Затем скопируйте HTTP API токен и вставьте его в консоль.");
        Console.Write("HTTP API Token: ");
        string? token = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Необходимо задать токен.");
        token = token.Trim();
        TgBotOptions options = new (token);
        string serializedOptions = JsonSerializer.Serialize(options);
        File.WriteAllText(ConfigurationVariables.TelegramBotTokenConfigPath, serializedOptions);
    }

    public static TgBotOptions LoadTgBotOptions()
    {
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(ConfigurationVariables.TelegramBotTokenConfigPath));
        document.RootElement.TryGetProperty("Token", out JsonElement token);
        string? tokenValue = token.GetString();
        if (string.IsNullOrWhiteSpace(tokenValue))
            throw new ApplicationException("Некорректный конфигурационный файл телеграм бота");
        return new TgBotOptions(tokenValue);
    }
}