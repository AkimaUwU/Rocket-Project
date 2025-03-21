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

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/task_create"]
    )]
    public async Task Create_Report_Task(ITelegramBotClient client, Update update)
    {
        string reply = """
            Вы попали в опцию создания задач.
            Для создания задачи, необходимо ввести сообщение с указанием даты.

            Например:

            завтра в 16:35 сходить в магазин,
            в четверг заказать пиццу в 16 35,
            13 марта купить пиццу в 16 35

            Вы можете покинуть опцию, введя /task_create_cancel
            """;

        await Message.Send(client, update, reply);
        update.RegisterStepHandler(new StepTelegram(HandleCreateReportTask));
    }

    public async Task HandleCreateReportTask(ITelegramBotClient client, Update update)
    {
        var message = update.Message;
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
        long messageId = message.Id;
        string trimmedMessage = messageText.Trim();
        Result<ReportTask> task = await _api.CreateNewTask(trimmedMessage, messageId);
        if (task.IsFailure)
        {
            await task.Error.Send(client, update);
            update.ClearStepUserHandler();
            return;
        }

        update.ClearStepUserHandler();
        string reply = $"""
            Задача создана:

            {task.Value}
            
            """;
        await Message.Send(client, update, reply);
    }
}
