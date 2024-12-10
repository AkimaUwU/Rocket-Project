using Telegram.Bot;
using Telegram.Bot.Types;

namespace RocketPlaner.TelegramBot.BotEndpoints.Abstractions;

public interface IBotEndpoint
{
    Task Handle(ITelegramBotClient client, Update update);
}
