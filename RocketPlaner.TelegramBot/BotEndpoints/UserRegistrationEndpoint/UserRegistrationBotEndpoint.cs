using System.Text;
using PRTelegramBot.Attributes;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Core.Tools;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RocketPlaner.TelegramBot.BotEndpoints.UserRegistrationEndpoint;

[BotHandler]
public class UserRegistrationBotEndpoint(ICommandDispatcher dispatcher) : IBotEndpoint
{
    [ReplyMenuHandler("Зарегистрироваться")]
    public async Task Handle(ITelegramBotClient client, Update update)
    {
        var dispatcherCopy = dispatcher;
        if (update.Message.From.IsBot)
        {
            return;
        }

        long? telegramId = update.Message.From.Id;
        var command = new RegisterUserCommand(telegramId);
        var result = await dispatcherCopy.Resolve<
            RegisterUserCommand,
            RocketPlaner.Core.models.Users.User
        >(command);

        if (result.IsError)
            await HandleFailure(client, update, result.Error);
        else
            await HandleSuccess(client, update);
    }

    private async Task HandleSuccess(ITelegramBotClient client, Update update)
    {
        var message = "Вы были успешно зарегистрированы!. Можете воспользоваться командой \"Меню\"";
        await PRTelegramBot.Helpers.Message.Send(client, update, message);
    }

    private async Task HandleFailure(ITelegramBotClient client, Update update, Error error)
    {
        StringBuilder builder = new StringBuilder(error.Text);
        builder.AppendLine("\nВоспользуйтесь командой \"Меню\"");
        string message = builder.ToString();
        await PRTelegramBot.Helpers.Message.Send(client, update, message);
    }
}
