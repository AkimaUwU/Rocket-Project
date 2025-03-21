using PRTelegramBot.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Shared.Extensions;

public static class TGUpdateExtensions
{
    public static Option<string> GetMessage(this Update update)
    {
        TGMessage? message = update.Message;

        if (message == null)
            return Option<string>.None();

        if (string.IsNullOrWhiteSpace(message.Text))
            return Option<string>.None();

        return Option<string>.Some(message.Text);
    }

    public static Option<long> GetUserId(this Update update)
    {
        TGMessage? message = update.Message;

        if (message == null)
            return Option<long>.None();

        if (message.From == null)
            return Option<long>.None();

        return Option<long>.Some(message.From.Id);
    }

    public static async Task RemoveLastMessage(this Update update, ITelegramBotClient client)
    {
        try
        {
            TGMessage? message = update.Message;
            if (message == null)
                return;

            int messageId = message.MessageId;
            await client.DeleteMessage(update.GetChatId(), messageId);
        }
        catch
        {
            // ignored
        }
    }

    public static async Task RemoveMessageById(this ITelegramBotClient client, TGMessage message)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        await client.DeleteMessage(chatId, messageId);
    }
}
