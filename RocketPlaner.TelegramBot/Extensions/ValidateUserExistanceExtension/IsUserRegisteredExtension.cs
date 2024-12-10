using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Queries.IsUserExist;
using RocketPlaner.Core.Tools;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RocketPlaner.TelegramBot.Extensions.ValidateUserExistanceExtension;

public static class IsUserRegisteredExtension
{
    public static async Task<Result<bool>> CheckIsUserRegistered(
        this IQueryDispatcher queryDispatcher,
        ITelegramBotClient client,
        Update update
    )
    {
        long? id = update.Message.From.Id;
        var query = new IsUserExistQuery(id);
        var result = await queryDispatcher.Resolve<IsUserExistQuery, bool>(query);
        if (result.IsError)
            await PRTelegramBot.Helpers.Message.Send(client, update, result.Error.Text);
        return result;
    }
}
