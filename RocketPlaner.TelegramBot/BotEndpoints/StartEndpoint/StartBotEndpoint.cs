using System.Text;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RocketPlaner.TelegramBot.BotEndpoints.StartEndpoint;

[BotHandler]
public sealed class StartBotEndpoint : IBotEndpoint
{
    [ReplyMenuHandler("/start")]
    public async Task Handle(ITelegramBotClient client, Update update)
    {
        var option = new OptionMessage();
        var menuList = new List<KeyboardButton>();
        menuList.Add(new KeyboardButton("Зарегистрироваться")); // кнопка обычная

        var menu = MenuGenerator.ReplyKeyboard(1, menuList, true);
        option.MenuReplyKeyboardMarkup = menu;
        await PRTelegramBot.Helpers.Message.Send(client, update, "Меню", option);
    }
}
