namespace ReportTaskPlanner.TelegramBot.Configuration;

public class ConfigurationVariables
{
    public static readonly string ConfigurationFolder =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration");

    public static readonly string TelegramBotTokenConfig =
        Path.Combine(ConfigurationFolder, "TG_BOT_CONFIG.json");
}