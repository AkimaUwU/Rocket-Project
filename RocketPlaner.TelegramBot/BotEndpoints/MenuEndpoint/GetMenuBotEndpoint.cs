using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Queries.IsUserExist;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using RocketPlaner.TelegramBot.Extensions.ValidateUserExistanceExtension;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RocketPlaner.TelegramBot.BotEndpoints.MenuEndpoint;

internal static class MenuCommands
{
    internal static string CreateTask = "Создать задачу";
    internal static string FetchMyTasks = "Просмотреть мои задачи";
}

[BotHandler]
public sealed class GetMenuBotEndpoint(IQueryDispatcher dispatcher) : IBotEndpoint
{
    [ReplyMenuHandler("Меню")]
    public async Task Handle(ITelegramBotClient client, Update update)
    {
        var isUserExists = await dispatcher.CheckIsUserRegistered(client, update);
        if (isUserExists.IsError)
            return;

        var message = "Меню приложения";
        var option = new OptionMessage();
        List<KeyboardButton> buttons = [];

        buttons.Add(new KeyboardButton(MenuCommands.FetchMyTasks));
        buttons.Add(new KeyboardButton(MenuCommands.CreateTask));
        var menu = MenuGenerator.ReplyKeyboard(1, buttons, true);
        option.MenuReplyKeyboardMarkup = menu;
        await PRTelegramBot.Helpers.Message.Send(client, update, message, option);
    }
}
