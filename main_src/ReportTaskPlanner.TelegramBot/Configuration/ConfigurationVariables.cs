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

    public static readonly string TimeZoneDbTokenConfigPath = Path.Combine(
        ConfigurationFolder,
        "TIME_ZONE_DB_CONFIG.json"
    );

    public static readonly string PlannerTimeConfigPath = Path.Combine(
        ConfigurationFolder,
        "PLANNER_TIME_CONFIG.json"
    );
}
