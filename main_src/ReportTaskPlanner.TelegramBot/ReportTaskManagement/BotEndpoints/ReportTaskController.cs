using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.BotEndpoints;

[BotHandler]
public sealed class ReportTaskController(ReportTaskManagementApi api)
{
    private readonly ReportTaskManagementApi _api = api;

    private const string CreateReportTaskStartReply = """
        Вы попали в опцию создания задач 👋.
        👉 Для создания задачи, необходимо ввести сообщение с указанием даты.

        ℹ️ Например:
        👉 завтра в 16:35 сходить в магазин,
        👉 в четверг заказать пиццу в 16 35,
        👉 13 марта купить пиццу в 16 35,
        👉 Сегодня будет встреча в 16 35,

        ℹ️ Бот понимает следующий формат времени:
        👉 ЧЧ:ММ,
        👉 ЧЧ ММ,
        👉 Ч:М,
        👉 Ч М,

        ℹ️ Бот понимает следующий виды дат:
        👉 Дни недели,
        👉 Относительные даты (сегодня, завтра, послезавтра),
        👉 Месяцы,

        ℹ️ Если Вы отправляете боту текст без времени, задача создана не будет.
        ℹ️ Бот понимает периодичность времени, при наличии слова - каждый.

        Вы можете покинуть опцию, введя /task_create_cancel
        """;

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/task_create"]
    )]
    public async Task Create_Report_Task(ITelegramBotClient client, Update update)
    {
        await Message.Send(client, update, CreateReportTaskStartReply);
        update.RegisterStepHandler(new StepTelegram(HandleCreateReportTask));
    }

    public async Task HandleCreateReportTask(ITelegramBotClient client, Update update)
    {
        TGMessage? message = update.Message;
        if (message == null)
            return;

        string? messageText = message.Text;
        if (string.IsNullOrWhiteSpace(messageText))
            return;

        if (messageText == "/task_create_cancel")
        {
            await Message.Send(client, update, "Отменено");
            update.ClearStepUserHandler();
            return;
        }

        await Message.Send(client, update, "Момент🕔...");
        await update.RemoveLastMessage(client);

        long messageId = message.Id;
        string trimmedMessage = messageText.Trim();
        Result<ReportTask> task = await _api.CreateNewTask(trimmedMessage, messageId);
        if (task.IsFailure)
        {
            await task.Error.Send(client, update);
            update.RegisterStepHandler(new StepTelegram(HandleCreateReportTask));
            return;
        }

        update.ClearStepUserHandler();
        string reply = $"""
            Задача создана ✅:

            👉 {task.Value}
            
            """;
        await Message.Send(client, update, reply);
    }
}
