using PRTelegramBot.Attributes;
using PRTelegramBot.Interfaces;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using RocketPlanner.TimeClassifier.Classificator;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RocketPlaner.TelegramBot.BotEndpoints.ClassifyInputEndpoint;

[BotHandler]
public class ClassifyInputBotEndpoint(TimeClassifier classifier) : IBotEndpoint
{
    [InlineCommand]
    private enum ClassifierButtons
    {
        OK = 201,
        Cancel = 202,
    }

    [ReplyMenuHandler(CommandComparison.Contains, "К:", "к:")]
    public async Task Handle(ITelegramBotClient client, Update update)
    {
        string message = update.Message!.Text!;
        string[] splittedMessage = message.Split(':', StringSplitOptions.RemoveEmptyEntries);
        string input = splittedMessage[1].Trim().ToLower();
        var engine = classifier.CreatePredictionEngine();
        var prediction = await engine.PredictAsync(input);

        var pressOk = new InlineCallback("Верно", ClassifierButtons.OK);
        var pressCancel = new InlineCallback("Неверно", ClassifierButtons.Cancel);

        var inlineButtons = new List<IInlineContent>();
        inlineButtons.Add(pressOk);
        inlineButtons.Add(pressCancel);

        var menu = MenuGenerator.InlineKeyboard(2, inlineButtons);
        var option = new OptionMessage();
        option.MenuInlineKeyboardMarkup = menu;

        await PRTelegramBot.Helpers.Message.Send(
            client,
            update,
            $"Класс: {prediction.PredictedLabel}. Точность: {prediction.Score.Max()}",
            option
        );
    }

    [InlineCallbackHandler<ClassifierButtons>(ClassifierButtons.OK)]
    public async Task HandlePressOk(ITelegramBotClient client, Update update)
    {
        string message = "Выбрано \'Верно\'. Сохранено в базу знаний";
        await PRTelegramBot.Helpers.Message.Send(client, update, message);
    }

    [InlineCallbackHandler<ClassifierButtons>(ClassifierButtons.Cancel)]
    public async Task HandlePressCancel(ITelegramBotClient client, Update update)
    {
        string message = "Выбрано \'Неверно\'. Удалено из базы знаний";
        await PRTelegramBot.Helpers.Message.Send(client, update, message);
    }
}
