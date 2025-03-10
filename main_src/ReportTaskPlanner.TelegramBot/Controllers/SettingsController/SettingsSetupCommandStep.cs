using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Controllers.SettingsController;

public sealed record SettingsSetupCommandStep : IExecuteStep
{
    private readonly Func<ITelegramBotClient, Update, Task> _operation;
    public SettingsSetupCache Cache { get; }
    
    public Func<ITelegramBotClient, Update, Task> GetExecuteMethod() => _operation;

    public SettingsSetupCommandStep(Func<ITelegramBotClient, Update, Task> operation, SettingsSetupCache cache) =>
        (_operation, Cache) = (operation, cache);

    public async Task<ExecuteStepResult> ExecuteStep(ITelegramBotClient botClient, Update update)
    {
        await _operation(botClient, update);
        LastStepExecuted = true;
        return ExecuteStepResult.Success;
    }

    public SettingsSetupCommandStep CreateContinueStep(Func<ITelegramBotClient, Update, Task> operation) =>
        new SettingsSetupCommandStep(operation, Cache);

    public bool IgnoreBasicCommands { get; set; }
    public bool LastStepExecuted { get; set; }
}