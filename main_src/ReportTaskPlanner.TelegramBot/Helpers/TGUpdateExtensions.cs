using PRTelegramBot.Extensions;
using ReportTaskPlanner.Utilities.OptionPattern;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Helpers;

public static class TGUpdateExtensions
{
    public static Option<string> GetMessage(this Update update)
    {
        TGMessage? message = update.Message;
        
        if (message == null)
            return Option<string>.None<string>();
        
        if (string.IsNullOrWhiteSpace(message.Text))
            return Option<string>.None<string>();
        
        return Option<string>.Some(message.Text);
    }

    public static Option<long> GetUserId(this Update update)
    {
        TGMessage? message = update.Message;
        
        if (message == null)
            return Option<long>.None<long>();
        
        if (message.From == null)
            return Option<long>.None<long>();
        
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
}