using ReportTaskPlanner.Utilities.ResultPattern;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Helpers;

public static class TGBotErrorExtensions
{
    public static async Task Send(this Error error, ITelegramBotClient client, Update update)
    {
        string text = $"""
            Ошибка:
            {error.Message}
            """;
        await Message.Send(client, update, text);
    }
}
