using PRTelegramBot.Attributes;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RocketPlaner.TelegramBot.BotEndpoints.TestEndpoint;

[BotHandler]
public class TestBotEndpoint : IBotEndpoint
{
    [ReplyMenuHandler("/test")]
    public async Task Handle(ITelegramBotClient client, Update update)
    {
        // Отправка команды напрямую в чат бота
        await PRTelegramBot.Helpers.Message.Send(client, update, "Test message");

        // Бот отправляет сообщение в чат группы
        //var chatId = new ChatId(-4742253341);
        var chatId = new ChatId(-4798589561);
        await client.SendMessage(chatId, "Test Message In Other chat");
    }
}
