using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Abstractions;

public abstract class BasicCommandExecuteStep : IExecuteStep
{
    private readonly Func<ITelegramBotClient, Update, Task> _operation;

    public BasicCommandExecuteStep(Func<ITelegramBotClient, Update, Task> operation) =>
        _operation = operation;

    public Func<ITelegramBotClient, Update, Task> GetExecuteMethod() => _operation;

    public async Task<ExecuteStepResult> ExecuteStep(ITelegramBotClient botClient, Update update)
    {
        await _operation.Invoke(botClient, update);
        return ExecuteStepResult.Success;
    }

    public bool IgnoreBasicCommands { get; set; }
    public bool LastStepExecuted { get; set; }
}
