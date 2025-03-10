using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Controllers.TimeZoneDbApiKeyManagement;

public sealed record TimeZoneDbApiKeySetupCommandStep : IExecuteStep
{
    private readonly Func<ITelegramBotClient, Update, Task> _operation;
    public TimeZoneDbApiKeySetupCache Cache { get; }

    public Func<ITelegramBotClient, Update, Task> GetExecuteMethod() => _operation;

    public TimeZoneDbApiKeySetupCommandStep(
        Func<ITelegramBotClient, Update, Task> operation,
        TimeZoneDbApiKeySetupCache cache
    ) => (_operation, Cache) = (operation, cache);

    public async Task<ExecuteStepResult> ExecuteStep(ITelegramBotClient botClient, Update update)
    {
        await _operation(botClient, update);
        LastStepExecuted = true;
        return ExecuteStepResult.Success;
    }

    public TimeZoneDbApiKeySetupCommandStep CreateContinueStep(
        Func<ITelegramBotClient, Update, Task> operation
    ) => new TimeZoneDbApiKeySetupCommandStep(operation, Cache);

    public bool IgnoreBasicCommands { get; set; }
    public bool LastStepExecuted { get; set; }
}
