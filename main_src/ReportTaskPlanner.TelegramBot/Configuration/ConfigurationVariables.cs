namespace ReportTaskPlanner.TelegramBot.Configuration;

public static class ConfigurationVariables
{
    public static readonly string ConfigurationFolder = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Configuration"
    );

    public static readonly string TelegramBotTokenConfigPath = Path.Combine(
        ConfigurationFolder,
        "TG_BOT_CONFIG.json"
    );
}
