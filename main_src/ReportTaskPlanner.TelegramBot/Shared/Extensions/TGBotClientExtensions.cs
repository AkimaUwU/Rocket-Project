using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Shared.Extensions;

public static class TGBotClientExtensions
{
    public static async Task RegisterCommandIfNotExists(
        this ITelegramBotClient client,
        string command,
        string description
    )
    {
        List<BotCommand> commands = (await client.GetMyCommands()).ToList();
        if (!commands.Any(c => c.Command == command))
        {
            commands.Add(new BotCommand() { Command = command, Description = description });
            await client.SetMyCommands(commands);
        }
    }
}
