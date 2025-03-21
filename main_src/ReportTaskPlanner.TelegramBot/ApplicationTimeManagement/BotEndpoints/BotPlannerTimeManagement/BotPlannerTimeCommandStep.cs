using ReportTaskPlanner.TelegramBot.TelegramBotManagement;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.BotEndpoints.BotPlannerTimeManagement;

public class BotPlannerTimeCommandStep : BasicCommandExecuteStep
{
    public BotPlannerTimeConfigurationCache Cache { get; }

    public BotPlannerTimeCommandStep(
        Func<ITelegramBotClient, Update, Task> operation,
        BotPlannerTimeConfigurationCache cache
    )
        : base(operation) => Cache = cache;

    public BotPlannerTimeCommandStep ContinueNext(
        Func<ITelegramBotClient, Update, Task> operation
    ) => new BotPlannerTimeCommandStep(operation, Cache);
}
