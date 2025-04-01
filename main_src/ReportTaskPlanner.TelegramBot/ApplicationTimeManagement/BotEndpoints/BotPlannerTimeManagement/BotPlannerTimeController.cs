using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Utils;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TelegramBotManagement;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.BotEndpoints.BotPlannerTimeManagement;

[BotHandler]
public sealed class BotPlannerTimeController
{
    private const string setupButton = "Настроить время ⚙️";
    private const string getCurrentTimeButton = "Узнать текущее время бота ❔";
    private const string leaveMenuButton = "Выйти из меню ↩️";
    private readonly BotPlannerTimeManagementApi _api;

    public BotPlannerTimeController(BotPlannerTimeManagementApi api) => _api = api;

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/time_config"]
    )]
    public async Task OnStart(ITelegramBotClient client, Update update)
    {
        await update.RemoveLastMessage(client);
        ReplyKeyboardMarkup menu = new ReplyKeyboardBuilder()
            .AddButton(setupButton)
            .AddButton(getCurrentTimeButton)
            .AddButton(leaveMenuButton)
            .Build(1);
        string replyMessage = """
            Вы попали в опцию конфигурирования времени бота 👋.
            Я выдал вам меню, для выбора конкретных операций 👉.
            """;
        OptionMessage option = new OptionMessageBuilder()
            .AddText(replyMessage)
            .AddReplyKeyboardMarkup(menu)
            .Build();
        TGMessage message = await Message.Send(client, update, replyMessage, option);
        BotPlannerTimeConfigurationCache cache = new BotPlannerTimeConfigurationCache();
        cache.AddMessage(message);
        BotPlannerTimeCommandStep command = new BotPlannerTimeCommandStep(ReplyDispatch, cache);
        update.RegisterStepHandler(command);
    }

    public async Task ReplyDispatch(ITelegramBotClient client, Update update)
    {
        Option<string> clientMessageOption = update.GetMessage();
        if (!clientMessageOption.HasValue)
            return;

        string clientMessage = clientMessageOption.Value;
        await update.RemoveLastMessage(client);
        BotPlannerTimeCommandStep? previous = update.GetStepHandler<BotPlannerTimeCommandStep>();
        if (previous == null)
            return;

        if (clientMessage == leaveMenuButton)
        {
            await OnLeave(client, update);
            return;
        }

        (TGMessage message, BotPlannerTimeCommandStep next) dispatchTuple = clientMessage switch
        {
            setupButton => (
                await Message.Send(client, update, setupButton),
                previous.ContinueNext(OnTimeConfigureStart)
            ),
            getCurrentTimeButton => (
                await Message.Send(client, update, getCurrentTimeButton),
                previous.ContinueNext(OnCurrentPlannerTimeRequest)
            ),
            leaveMenuButton => (
                await Message.Send(client, update, leaveMenuButton),
                previous.ContinueNext(OnLeave)
            ),
            _ => (
                await Message.Send(client, update, string.Intern("Меню времени бота.")),
                previous.ContinueNext(ReplyDispatch)
            ),
        };

        await client.RemoveMessageById(dispatchTuple.message);
        await dispatchTuple.next.ExecuteStep(client, update);
    }

    public async Task OnTimeConfigureStart(ITelegramBotClient client, Update update)
    {
        BotPlannerTimeCommandStep? previous = update.GetStepHandler<BotPlannerTimeCommandStep>();
        if (previous == null)
            return;

        using (
            new MessageAwaiter(
                client,
                update.GetChatId(),
                string.Intern("Получаю список временных зон 🕔...")
            )
        )
        {
            ApplicationTime[] times = await _api.ListTimeZones();
            if (times.Length == 0)
            {
                await Message.Send(
                    client,
                    update,
                    string.Intern(
                        "Не удалось получить временные зоны. Возможно Time Zone Db провайдер не был настроен ❌."
                    )
                );
                return;
            }

            previous.Cache.AddTimes(times);

            List<List<KeyboardButton>> timeButtons = [];
            string leaveMenuText = string.Intern("\ud83d\udeab Покинуть выбор");
            timeButtons.Add([new KeyboardButton(leaveMenuText)]);
            for (int i = 0; i < times.Length; i += 2)
            {
                ReplyKeyboardBuilder builder = new ReplyKeyboardBuilder();
                ApplicationTime time = times[i];
                string selectTimeText = $"\ud83d\udc49 {time.DisplayName} / {time.DateString}";
                builder = builder.AddButton(selectTimeText);
                if (i + 1 < times.Length)
                {
                    time = times[i + 1];
                    selectTimeText = $"\ud83d\udc49 {time.DisplayName} / {time.DateString}";
                    builder = builder.AddButton(selectTimeText);
                }

                timeButtons.Add(builder.Buttons.ToList());
            }

            timeButtons.Add([new KeyboardButton(leaveMenuText)]);

            string replyMessage = string.Intern("Выберите временную зону 👉:");
            OptionMessage option = new OptionMessageBuilder()
                .AddText(replyMessage)
                .AddReplyKeyboardMarkup(new ReplyKeyboardMarkup(timeButtons))
                .Build();
            previous.Cache.AddMessage(await Message.Send(client, update, replyMessage, option));
            update.RegisterStepHandler(previous.ContinueNext(OnTimeZoneSelected));
        }
    }

    public async Task OnTimeZoneSelected(ITelegramBotClient client, Update update)
    {
        Option<string> clientReplyOption = update.GetMessage();
        if (!clientReplyOption.HasValue)
            return;

        BotPlannerTimeCommandStep? previous = update.GetStepHandler<BotPlannerTimeCommandStep>();
        if (previous == null)
            return;

        if (update.Message != null)
            previous.Cache.AddMessage(update.Message);

        string clientReply = clientReplyOption.Value;
        if (clientReply.Contains("Покинуть", StringComparison.OrdinalIgnoreCase))
        {
            await OnStart(client, update);
            return;
        }

        string[] splittedReply = clientReply.Split('/');
        string cityPart = splittedReply[0].Trim();
        string[] cityPartSplitted = cityPart.Split(' ');
        string cityValue = cityPartSplitted[^1];

        Result<ApplicationTime> time = previous.Cache.Get(cityValue);
        if (time.IsFailure)
        {
            string error = string.Intern("Вам нужно выбрать временную зону из меню ℹ️.");
            await Message.Send(client, update, error);
            update.RegisterStepHandler(previous.ContinueNext(OnTimeZoneSelected));
            return;
        }

        await _api.SaveTime(time);
        string replyMessage = $"""
            Вы установили временную зону бота ✅:
            {time.Value.DisplayName}
            """;

        previous.Cache.ClearData();
        ReplyKeyboardMarkup menu = new ReplyKeyboardBuilder()
            .AddButton(setupButton)
            .AddButton(getCurrentTimeButton)
            .AddButton(leaveMenuButton)
            .Build(1);
        OptionMessage option = new OptionMessageBuilder()
            .AddText(replyMessage)
            .AddReplyKeyboardMarkup(menu)
            .Build();
        TGMessage message = await Message.Send(client, update, replyMessage, option);
        BotPlannerTimeConfigurationCache cache = new BotPlannerTimeConfigurationCache();
        cache.AddMessage(message);
        BotPlannerTimeCommandStep command = new BotPlannerTimeCommandStep(ReplyDispatch, cache);
        update.RegisterStepHandler(command);
    }

    public async Task OnLeave(ITelegramBotClient client, Update update)
    {
        OptionMessage messageOptions = new OptionMessage();
        messageOptions.ClearMenu = true;
        TGMessage message = await Message.Send(client, update, "Выход из меню", messageOptions);
        await client.RemoveMessageById(message);
        update.ClearStepUserHandler();
    }

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/bot_time"]
    )]
    public async Task OnCurrentPlannerTimeRequest(ITelegramBotClient client, Update update)
    {
        Option<ApplicationTime> time = await _api.GetCurrentAppTime();
        if (!time.HasValue)
        {
            Error error = new Error("Время приложения не было установлено ❌.");
            await error.Send(client, update);
            return;
        }

        Option<TimeZoneDbOptions> tzOptions = await _api.GetTimeZoneDbOptions();
        if (!tzOptions.HasValue)
        {
            Error error = new Error("Конфигурация Time Zone Db провайдера не была установлена ❌.");
            await error.Send(client, update);
            return;
        }

        Result<ApplicationTime> currentTime = await _api.UpdateTime(tzOptions.Value, time.Value);
        if (currentTime.IsFailure)
        {
            await currentTime.Error.Send(client, update);
            return;
        }

        ApplicationTime updated = currentTime.Value;

        string replyMessage = $"""
            Текущее время бота 👉:
            Регион: {updated.DisplayName}
            Время: {updated.DateTime.ToString("HH:mm:ss  dd.MM.yyyy")}
            """;
        await Message.Send(client, update, replyMessage);
    }
}
