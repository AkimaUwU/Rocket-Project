using System.Globalization;
using System.Text;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Interfaces;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.AddTaskForUsers;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using RocketPlaner.TelegramBot.BotEndpoints.MenuEndpoint;
using RocketPlaner.TelegramBot.Extensions.ValidateUserExistanceExtension;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = PRTelegramBot.Helpers.Message;

namespace RocketPlaner.TelegramBot.BotEndpoints.CreateTaskEndpoint;

[BotHandler]
public sealed class CreateTaskBotEndpoint(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher,
    CreateTaskStateContainer container
) : IBotEndpoint
{
    [InlineCommand]
    internal enum Buttons
    {
        Single = 100,
        Periodic = 101,
    }

    [InlineCommand]
    internal enum Calendar
    {
        Calendar = 102,
    }

    [ReplyMenuHandler("Создать задачу")]
    public async Task Handle(ITelegramBotClient client, Update update)
    {
        var isUserExists = await queryDispatcher.CheckIsUserRegistered(client, update);
        if (isUserExists.IsError)
            return;

        container.Clean();
        var id = update.Message.From.Id;
        container.SetUserTelegramId(id);
        var selectOneLife = new InlineCallback("Одноразовая", Buttons.Single);
        var selectMultiLife = new InlineCallback("Периодическая", Buttons.Periodic);
        var list = new List<IInlineContent>();
        list.Add(selectOneLife);
        list.Add(selectMultiLife);
        var menu = MenuGenerator.InlineKeyboard(2, list);
        var option = new OptionMessage();
        option.MenuInlineKeyboardMarkup = menu;
        var message = await PRTelegramBot.Helpers.Message.Send(
            client,
            update,
            "Выберите тип задачи",
            option
        );
        container.SetMessageId(message.MessageId);
    }

    [InlineCallbackHandler<Buttons>(Buttons.Single)]
    public async Task HandleOneLifeSelection(ITelegramBotClient client, Update update)
    {
        container.SetSelectedTaskType(RocketTaskType.Single);
        await PRTelegramBot.Helpers.Message.Send(client, update, "Выбрана одноразовая задача");
        if (container.MessageId != 0)
        {
            await PRTelegramBot.Helpers.Message.DeleteMessage(
                client,
                update.GetChatId(),
                container.MessageId
            );
            await PRTelegramBot.Helpers.Message.Send(client, update, "Напишите заголовок задачи");
            update.RegisterStepHandler(new PRTelegramBot.Models.StepTelegram(HandleTaskTitleType));
        }
    }

    [InlineCallbackHandler<Buttons>(Buttons.Periodic)]
    public async Task HandleMultiLifeSelection(ITelegramBotClient client, Update update)
    {
        container.SetSelectedTaskType(RocketTaskType.Periodic);
        await PRTelegramBot.Helpers.Message.Send(client, update, "Выбрана периодическая задача");
        if (container.MessageId != 0)
        {
            await PRTelegramBot.Helpers.Message.DeleteMessage(
                client,
                update.GetChatId(),
                container.MessageId
            );
            await PRTelegramBot.Helpers.Message.Send(client, update, "Напишите заголовок задачи:");
            update.RegisterStepHandler(new PRTelegramBot.Models.StepTelegram(HandleTaskTitleType));
        }
    }

    private async Task HandleTaskTitleType(ITelegramBotClient client, Update update)
    {
        var lastMessage = update.Message.Text;
        container.SetTaskTitle(lastMessage);
        update.ClearStepUserHandler();
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"\n Заголовок задачи: {lastMessage}");
        var response = stringBuilder.ToString();
        await PRTelegramBot.Helpers.Message.Send(client, update, response);
        await PRTelegramBot.Helpers.Message.Send(client, update, "Напишите текст задачи:");
        update.RegisterStepHandler(
            new PRTelegramBot.Models.StepTelegram(HandleTaskDescriptionType)
        );
    }

    private async Task HandleTaskDescriptionType(ITelegramBotClient client, Update update)
    {
        var lastMessage = update.Message.Text;
        container.SetTaskDescription(lastMessage);
        update.ClearStepUserHandler();
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"\nТекст задачи: {lastMessage}");
        var response = stringBuilder.ToString();
        await PRTelegramBot.Helpers.Message.Send(client, update, response);
        await CalendarUtils.Create(
            client,
            update,
            CultureInfo.GetCultureInfo("ru-RU"),
            Calendar.Calendar,
            "Выберите дату: "
        );
    }

    [InlineCallbackHandler<Calendar>(Calendar.Calendar)]
    public async Task HandleDateSelection(ITelegramBotClient client, Update update)
    {
        var bot = client.GetBotDataOrNull();
        try
        {
            using (var inlineHandler = new InlineCallback<CalendarTCommand>(client, update))
            {
                var command = inlineHandler.GetCommandByCallbackOrNull();
                container.SetSelectedDate(command.Data.Date);
                var stringBuilder = new StringBuilder();
                var response = command.Data.Date.ToString().Split(' ')[0];
                stringBuilder.AppendLine($"\nВыбранная дата: {response}");
                await PRTelegramBot.Helpers.Message.Send(
                    client,
                    update,
                    command.Data.Date.ToString()
                );
                await PRTelegramBot.Helpers.Message.Send(
                    client,
                    update,
                    "Введите время в формате: \"ЧЧ:ММ\""
                );
                update.RegisterStepHandler(new PRTelegramBot.Models.StepTelegram(HandleTimeInput));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            container.Clean();
        }
    }

    private async Task HandleTimeInput(ITelegramBotClient client, Update update)
    {
        try
        {
            var lastMessage = update.Message.Text;
            update.ClearStepUserHandler();
            var stringBuilder = new StringBuilder(lastMessage);
            stringBuilder.Append(":00");
            var dateWithTime = DateTime.ParseExact(
                stringBuilder.ToString(),
                "HH:mm:ss",
                CultureInfo.InvariantCulture
            );
            var dateFromContainer = container.SelectedDate;
            dateFromContainer = dateFromContainer.AddHours(dateWithTime.Hour);
            dateFromContainer = dateFromContainer.AddMinutes(dateWithTime.Minute);
            container.SetSelectedDate(dateFromContainer);
            var responseBuilder = new StringBuilder();
            responseBuilder.AppendLine($"Итоговая дата: {container.SelectedDate.ToString()}");
            await PRTelegramBot.Helpers.Message.Send(client, update, responseBuilder.ToString());
            await PRTelegramBot.Helpers.Message.Send(client, update, "Задача создается...");
            await FinishTaskCreation(client, update);
        }
        catch
        {
            container.Clean();
            await PRTelegramBot.Helpers.Message.Send(
                client,
                update,
                "Произошла ошибка при чтении даты. Повторите попытку снова."
            );
        }
    }

    private async Task FinishTaskCreation(ITelegramBotClient client, Update update)
    {
        var command = new AddTaskForUsersCommand(
            container.TaskTitle,
            container.TaskDescription,
            container.TaskType,
            container.SelectedDate,
            container.UserTelegramId
        );
        var result = await commandDispatcher.Resolve<AddTaskForUsersCommand, RocketTask>(command);
        if (result.IsError)
        {
            await PRTelegramBot.Helpers.Message.Send(client, update, result.Error.Text);
            container.Clean();
        }

        var responseBuilder = new StringBuilder();
        responseBuilder.AppendLine("\nСоздана задача:");
        responseBuilder.AppendLine($"\nЗаголовок: {result.Value.Title.Title}");
        responseBuilder.AppendLine($"\nТекст: {result.Value.Message.Message}");
        responseBuilder.AppendLine($"\nТип задачи: {result.Value.Type.Type.ToString()}");
        responseBuilder.AppendLine(
            $"\nДата уведомления: {result.Value.FireDate.FireDate.ToString()}"
        );
        await PRTelegramBot.Helpers.Message.Send(client, update, responseBuilder.ToString());
        container.Clean();
    }
}
