using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Utils;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.BotEndpoints.TimeZoneDbApiKeyManagement;

[BotHandler]
public sealed record TimeZoneDbApiKeySetupController
{
    public const string ContinueStepButtonText = "Продолжить";
    public const string CancelSessionButtonText = "Отменить";
    private readonly BotPlannerTimeManagementApi _api;

    public TimeZoneDbApiKeySetupController(BotPlannerTimeManagementApi api) => _api = api;

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/start", "/update_time_api_key"]
    )]
    public async Task OnStart(ITelegramBotClient client, Update update)
    {
        Option<string> startMessageOption = update.GetMessage();
        if (!startMessageOption.HasValue)
            return;

        await update.RemoveLastMessage(client);
        string startMessage = startMessageOption.Value;
        if (startMessage == "/start")
        {
            Option<TimeZoneDbOptions> tzOptions = await _api.GetTimeZoneDbOptions();
            if (tzOptions.HasValue)
            {
                await client.RegisterCommandIfNotExists(
                    "/update_time_api_key",
                    "Опция обновления Time Zone Db ключа"
                );
                await client.RegisterCommandIfNotExists(
                    "/has_time_api_key",
                    "Опция проверки Time Zone Db ключа"
                );
                await client.RegisterCommandIfNotExists("/time_config", "Управление временем бота");
                await client.RegisterCommandIfNotExists("/bot_time", "Текущее время бота");
                await client.RegisterCommandIfNotExists("/task_create", "Создание уведомления");
                await client.RegisterCommandIfNotExists(
                    "/add_this_chat",
                    "Вызовите это команду в чате, с которым надо связать бота, предварительно пригласив бота в чат."
                );
                await Message.Send(
                    client,
                    update,
                    "Добро пожаловать назад в бот-планировщик задач"
                );
                return;
            }
        }

        string replyMessage = startMessage switch
        {
            "/start" => """
                Вас приветствует телеграм-бот планировщик задач.

                Для корректной работы бота необходимо выполнить первоначальную настройку.

                Бот взаимодействует с сервисом Time Zone Db.

                Зарегистрируйтесь на https://timezonedb.com чтобы получить ключ, который понадобится для настройки бота.

                Чтобы остановить настройку, Вы можете написать команду: /stop_config.
                Либо воспользуйтесь кнопкой Отменить в меню.

                Когда Вы зарегистрируетесь нажмите Продолжить
                """,
            "/update_time_api_key" => """
                Вы попали в опцию изменения токена Time Zone Db.

                Для изменения токена, Вы должны быть зарегистрированы на платформе:

                https://timezonedb.com

                Если вы зарегистрированы на платформе, нажмите Продолжить.
                """,
            _ => throw new ArgumentException("Unsupported settings controller command"),
        };

        List<KeyboardButton> settingsSetupMenu = new List<KeyboardButton>();
        settingsSetupMenu.Add(new KeyboardButton(ContinueStepButtonText));
        settingsSetupMenu.Add(new KeyboardButton(CancelSessionButtonText));
        var menu = MenuGenerator.ReplyKeyboard(1, settingsSetupMenu);
        OptionMessage options = new OptionMessage() { MenuReplyKeyboardMarkup = menu };

        TimeZoneDbApiKeySetupCache cache = new TimeZoneDbApiKeySetupCache();
        long userId = update.Message!.From!.Id;
        cache.RegisterStep(userId);
        cache.UpdateStep(userId, TimeZoneDbApiKeySetupCache.start);
        cache.AddMessage(await Message.Send(client, update, replyMessage, options));

        TimeZoneDbApiKeySetupCommandStep step = new TimeZoneDbApiKeySetupCommandStep(
            OnContinue,
            cache
        );
        update.RegisterStepHandler(step);
    }

    public async Task OnContinue(ITelegramBotClient client, Update update)
    {
        await update.RemoveLastMessage(client);

        TimeZoneDbApiKeySetupCommandStep? previous =
            update.GetStepHandler<TimeZoneDbApiKeySetupCommandStep>();
        if (previous == null)
            return;

        Option<long> userIdOption = update.GetUserId();
        if (!userIdOption.HasValue)
            return;

        long userId = userIdOption.Value;
        if (!previous.Cache.HasStep(userId))
            return;

        Option<string> textOption = update.GetMessage();
        if (!textOption.HasValue)
            return;

        string text = textOption.Value;
        if (text == CancelSessionButtonText)
        {
            await OnCancel(client, update);
            return;
        }

        if (text != ContinueStepButtonText)
        {
            update.RegisterStepHandler(previous.CreateContinueStep(OnContinue));
            return;
        }

        OptionMessage options = new OptionMessage();
        options.ClearMenu = true;
        previous.Cache.AddMessage(
            await Message.Send(client, update, "Продолжаем настройку", options)
        );

        string replyMessage = """
            Хорошо, теперь, отправьте мне API токен (API Key).

            Найти API токен от Time Zone Db можно по следующей ссылке:

            https://timezonedb.com/account
            """;

        List<KeyboardButton> settingsSetupMenu = new List<KeyboardButton>();
        settingsSetupMenu.Add(new KeyboardButton(CancelSessionButtonText));
        var menu = MenuGenerator.ReplyKeyboard(1, settingsSetupMenu);
        options.ClearMenu = false;
        options.MenuReplyKeyboardMarkup = menu;
        previous.Cache.AddMessage(await Message.Send(client, update, replyMessage, options));
        previous.Cache.UpdateStep(userId, TimeZoneDbApiKeySetupCache.waitsForToken);

        TimeZoneDbApiKeySetupCommandStep next = previous.CreateContinueStep(OnClientTokenReply);
        update.RegisterStepHandler(next);
    }

    public async Task OnClientTokenReply(ITelegramBotClient client, Update update)
    {
        await update.RemoveLastMessage(client);

        TimeZoneDbApiKeySetupCommandStep? previous =
            update.GetStepHandler<TimeZoneDbApiKeySetupCommandStep>();
        if (previous == null)
            return;

        long userId = update.Message!.From!.Id;
        if (!previous.Cache.HasStep(userId))
            return;

        Option<string> textOption = update.GetMessage();
        if (!textOption.HasValue)
            return;

        string text = textOption.Value;
        if (text == CancelSessionButtonText)
        {
            await OnCancel(client, update);
            return;
        }

        foreach (var character in text)
        {
            if (char.IsDigit(character))
                continue;
            if (!char.IsUpper(character))
            {
                string errorMessage = """
                    Этот текст не похож на токен из Time Zone Db.
                    Time Zone Db должен предоставить токен со со случайной последовательностью символов в верхнем регистре.
                    """;

                previous.Cache.AddMessage(await Message.Send(client, update, errorMessage));
                TimeZoneDbApiKeySetupCommandStep next = previous.CreateContinueStep(
                    OnClientTokenReply
                );
                update.RegisterStepHandler(next);
                return;
            }
        }

        Result<TimeZoneDbOptions> saving = await _api.SetTimeZoneDbOptions(text);
        if (!saving.IsSuccess)
        {
            string errorMessage = $"""
                Произошла ошибка во время сохранения токена.
                Текст ошибки - {saving.Error.Message}.
                """;

            previous.Cache.AddMessage(await Message.Send(client, update, errorMessage));
        }

        IReadOnlyCollection<int> previousMessages = previous.Cache.MessageIds;
        foreach (int messageId in previousMessages)
            await client.DeleteMessage(update.GetChatId(), messageId);

        OptionMessage replyMessageOption = new OptionMessage() { ClearMenu = true };
        string replyMessage = """
            Отлично. Я сохранил токен Time Zone Db в своей конфигурации.
            Данный токен является бессрочным.

            Теперь, после того, как я знаю ключ от Time Zone Db сервиса,
            Вы можете установить мне время, в котором я буду работать.

            Для этого вызовите команду /time_config

            Вы можете в любое время поменять токен Time Zone Db.
            Для этого вызовите команду /update_time_api_key
            """;

        if (update.HasStepHandler())
            update.ClearStepUserHandler();

        await Message.Send(client, update, replyMessage, replyMessageOption);

        await client.RegisterCommandIfNotExists(
            "/update_time_api_key",
            "Опция обновления Time Zone Db ключа"
        );
        await client.RegisterCommandIfNotExists(
            "/has_time_api_key",
            "Опция проверки Time Zone Db ключа"
        );
        await client.RegisterCommandIfNotExists("/time_config", "Управление временем бота");
        await client.RegisterCommandIfNotExists("/bot_time", "Текущее время бота");
        await client.RegisterCommandIfNotExists("/task_create", "Создание уведомления");
        await client.RegisterCommandIfNotExists(
            "/add_this_chat",
            "Вызовите это команду в чате, с которым надо связать бота, предварительно пригласив бота в чат."
        );
    }

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/stop_config"]
    )]
    public async Task OnCancel(ITelegramBotClient client, Update update)
    {
        await update.RemoveLastMessage(client);

        string message = """
            Этап настройки бота остановлен.
            """;

        if (update.HasStepHandler())
            update.ClearStepUserHandler();

        OptionMessage options = new OptionMessage() { ClearMenu = true };
        await Message.Send(client, update, message, options);
    }

    [ReplyMenuHandler(
        CommandComparison.Contains,
        StringComparison.OrdinalIgnoreCase,
        ["/has_time_api_key"]
    )]
    public async Task HasApiToken(ITelegramBotClient client, Update update)
    {
        await update.RemoveLastMessage(client);
        Option<TimeZoneDbOptions> option = await _api.GetTimeZoneDbOptions();
        _ = option.HasValue switch
        {
            true => await Message.Send(
                client,
                update,
                "В боте существует конфигурация Time Zone Db."
            ),
            false => await Message.Send(
                client,
                update,
                "В боте отсутствует конфигурация Time Zone Db."
            ),
        };
    }
}
