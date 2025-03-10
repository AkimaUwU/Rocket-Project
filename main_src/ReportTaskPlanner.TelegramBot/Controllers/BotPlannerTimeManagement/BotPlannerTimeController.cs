using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Utils;
using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.TelegramBot.Configuration;
using ReportTaskPlanner.TelegramBot.Helpers;
using ReportTaskPlanner.TimeZoneDbProvider.Configuration;
using ReportTaskPlanner.TimeZoneDbProvider.TimeZoneDbProviderIntegration;
using ReportTaskPlanner.Utilities.OptionPattern;
using ReportTaskPlanner.Utilities.ResultPattern;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.Controllers.BotPlannerTimeManagement;

[BotHandler]
public sealed class BotPlannerTimeController
{
    private const string setupButton = "Настроить время";
    private const string getCurrentTimeButton = "Узнать текущее время бота";
    private const string leaveMenuButton = "Выйти из меню";

    private readonly TimeZoneDbRequester _requester;
    private readonly TimeZoneDbOptionsFileRepository _repository;
    private readonly TimeZoneDbOptionsJsonReader _reader;
    private readonly ILogger _logger;

    private readonly ReplyKeyboardMarkup _mainMenu = MenuGenerator.ReplyKeyboard(
        1,
        new List<KeyboardButton>()
        {
            new KeyboardButton(setupButton),
            new KeyboardButton(getCurrentTimeButton),
            new KeyboardButton(leaveMenuButton),
        }
    );

    public BotPlannerTimeController(
        TimeZoneDbRequester requester,
        TimeZoneDbOptionsFileRepository repository,
        TimeZoneDbOptionsJsonReader reader,
        ILogger logger
    ) => (_requester, _repository, _reader, _logger) = (requester, repository, reader, logger);

    [ReplyMenuHandler(
        CommandComparison.Equals,
        StringComparison.OrdinalIgnoreCase,
        ["/time_config"]
    )]
    public async Task OnStart(ITelegramBotClient client, Update update)
    {
        await update.RemoveLastMessage(client);

        string replyMessage = """
            Вы попали в опцию конфигурирования времени бота.

            Я выдал вам меню, для выбора конкретных операций.
            """;

        OptionMessage messageOptions = new OptionMessage();
        messageOptions.MenuReplyKeyboardMarkup = _mainMenu;
        TGMessage message = await Message.Send(client, update, replyMessage, messageOptions);
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
                await Message.Send(client, update, "Меню времени бота."),
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
            MessageAwaiter messageAwaiter = new MessageAwaiter(
                client,
                update.GetChatId(),
                "Получаю список временных зон..."
            )
        )
        {
            Result<TimeZoneDbOptions> options = _repository.Get(
                ConfigurationVariables.TimeZoneDbTokenConfigPath
            );
            if (options.IsSuccess == false)
            {
                string error = $"""
                    Ошибка
                    {options.Error.Message}
                    """;
                await Message.Send(client, update, error);
                return;
            }

            Result<TimeZoneDbResponseJson> json = await _requester.RequestJson(options);
            if (json.IsSuccess == false)
            {
                string error = $"""
                    Ошибка
                    {json.Error.Message}
                    """;
                await Message.Send(client, update, error);
                return;
            }

            Result<IEnumerable<PlannerTime>> plannerTimeResult = _requester
                .ReadJson(json, _reader.Read)
                .Invoke();
            if (plannerTimeResult.IsSuccess == false)
            {
                string error = $"""
                    Ошибка
                    {plannerTimeResult.Error.Message}
                    """;
                await Message.Send(client, update, error);
                return;
            }

            PlannerTime[] timesArray = plannerTimeResult.Value.ToArray();
            List<List<KeyboardButton>> timeButtons = [];
            timeButtons.Add([new("\ud83d\udeab Покинуть выбор")]);
            for (int i = 0; i < timesArray.Length; i += 2)
            {
                List<KeyboardButton> row = new List<KeyboardButton>();
                PlannerTime time = timesArray[i];
                row.Add(
                    new KeyboardButton(
                        $"\ud83d\udc49 {time.DisplayName} / {time.DateTime.ToString("HH:mm:ss / dd.MM.yyyy")}"
                    )
                );
                if (i + 1 < timesArray.Length)
                {
                    time = timesArray[i + 1];
                    row.Add(
                        new KeyboardButton(
                            $"\ud83d\udc49  {time.DisplayName} / {time.DateTime.ToString("HH:mm:ss  dd.MM.yyyy")}"
                        )
                    );
                }
                timeButtons.Add(row);
            }
            timeButtons.Add([new("\ud83d\udeab Покинуть выбор")]);

            OptionMessage replyOptions = new OptionMessage();
            replyOptions.MenuReplyKeyboardMarkup = new ReplyKeyboardMarkup(timeButtons);
            previous.Cache.AddMessage(
                await Message.Send(client, update, "Выберите временную зону:", replyOptions)
            );
            previous.Cache.AddTimes(timesArray);
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

        PlannerTime? time = previous.Cache.Times.FirstOrDefault(t => t.DisplayName == cityValue);
        if (time == null)
        {
            await Message.Send(client, update, "Вам нужно выбрать временную зону из меню.");
            update.RegisterStepHandler(previous.ContinueNext(OnTimeZoneSelected));
            return;
        }

        string filePath = ConfigurationVariables.PlannerTimeConfigPath;
        if (File.Exists(filePath))
            File.Delete(filePath);

        Result saving = _requester.SavePlannerTime(time, filePath);
        if (saving.IsSuccess == false)
        {
            await saving.Error.Send(client, update);
            update.RegisterStepHandler(previous.ContinueNext(OnTimeZoneSelected));
            return;
        }
        _logger.Information(
            "{Context}. Saved time zone settings: {Time}",
            nameof(BotPlannerTimeController),
            time
        );

        previous.Cache.ClearData();
        string replyMessage = $"""
            Вы установили временную зону бота:
            {time.DisplayName}
            """;

        OptionMessage messageOptions = new OptionMessage();
        messageOptions.MenuReplyKeyboardMarkup = _mainMenu;
        TGMessage message = await Message.Send(client, update, replyMessage, messageOptions);
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

    [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, ["/bot_time"])]
    public async Task OnCurrentPlannerTimeRequest(ITelegramBotClient client, Update update)
    {
        Result<PlannerTime> time = _requester.GetFromJsonFile(
            ConfigurationVariables.PlannerTimeConfigPath
        );
        if (time.IsSuccess == false)
        {
            await time.Error.Send(client, update);
            return;
        }

        Result<TimeZoneDbOptions> options = _repository.Get(
            ConfigurationVariables.TimeZoneDbTokenConfigPath
        );
        if (options.IsSuccess == false)
        {
            await options.Error.Send(client, update);
            return;
        }

        Result<PlannerTime> updated = await _requester.UpdatePlannerTime(
            time,
            _reader.Read,
            options
        );
        if (updated.IsSuccess == false)
        {
            await updated.Error.Send(client, update);
            return;
        }

        _requester.SavePlannerTime(updated, ConfigurationVariables.PlannerTimeConfigPath);
        string replyMessage = $"""
            Текущее время бота:
            Регион: {updated.Value.DisplayName}
            Время: {updated.Value.DateTime.ToString("HH:mm:ss  dd.MM.yyyy")}
            """;
        await Message.Send(client, update, replyMessage);
    }
}
