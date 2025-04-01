using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.BotEndpoints;

[BotHandler]
public sealed class TaskReceiversManagementController(TaskReceiversManagementApi api)
{
    private readonly TaskReceiversManagementApi _api = api;

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        commands: ["/add_this_chat"]
    )]
    public async Task RegisterChat(ITelegramBotClient client, Update update)
    {
        var message = update.Message;
        if (message == null)
            return;

        long chatId = message.Chat.Id;
        Result<TaskReceiver> result = await _api.RegisterReceiver(chatId);
        if (result.IsFailure)
        {
            await result.Error.Send(client, update);
            return;
        }

        await Message.Send(
            client,
            update,
            $"Чат с ID: {chatId} был добавлен в Tg Bot Task Planner"
        );
    }

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        commands: ["/remove_this_chat"]
    )]
    public async Task RemoveChat(ITelegramBotClient client, Update update)
    {
        var message = update.Message;
        if (message == null)
            return;

        long chatId = message.Chat.Id;
        Result<bool> removed = await _api.RemoveReceiver(chatId);
        if (removed.IsFailure)
        {
            await removed.Error.Send(client, update);
            return;
        }

        await Message.Send(
            client,
            update,
            $"Чат с ID: {removed.Value} удален из Tg Bot Task Planner"
        );
    }
}
