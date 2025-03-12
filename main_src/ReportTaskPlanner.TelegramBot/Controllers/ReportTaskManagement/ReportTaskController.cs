using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using ReportTaskPlanner.DependencyInjection;
using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.RegexDateTimeParser.Types.DateTypes;
using ReportTaskPlanner.RegexDateTimeParser.Types.RegexMatches;
using ReportTaskPlanner.RegexDateTimeParser.Types.RegexTypes;
using ReportTaskPlanner.TelegramBot.Helpers;
using ReportTaskPlanner.UseCases.ReportTaskManagement.CreateReportTaskUseCase;
using ReportTaskPlanner.Utilities.Common;
using ReportTaskPlanner.Utilities.OptionPattern;
using ReportTaskPlanner.Utilities.ResultPattern;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReportTaskPlanner.TelegramBot.Controllers.ReportTaskManagement;

[BotHandler]
public sealed class ReportTaskController
{
    private readonly RequestDispatcher _dispatcher;

    public ReportTaskController(RequestDispatcher dispatcher) => _dispatcher = dispatcher;

    [ReplyMenuHandler(
        CommandComparison.Equals,
        StringComparison.OrdinalIgnoreCase,
        ["/task_create"]
    )]
    public async Task Create_Report_Task(ITelegramBotClient client, Update update)
    {
        string reply = """
            Вы попали в опцию создания задач.
            Для создания задачи, необходимо ввести сообщение с указанием даты.

            Например:

            завтра в 16:35 сходить в магазин,
            в четверг заказать пиццу в 16 35,
            13 марта купить пиццу в 16 35

            Вы можете покинуть опцию, введя /task_create_cancel
            """;

        await Message.Send(client, update, reply);
        update.RegisterStepHandler(new StepTelegram(HandleCreateReportTask));
    }

    public async Task HandleCreateReportTask(ITelegramBotClient client, Update update)
    {
        Option<string> clientMessage = update.GetMessage();
        if (!clientMessage.HasValue)
            return;

        string messageText = clientMessage.Value.RemoveExtraSpaces();
        if (messageText == "/task_create_cancel")
        {
            await Message.Send(client, update, "Отменено");
            update.ClearStepUserHandler();
            return;
        }

        RegexType[] regexTypes = RegexTypeList.Array;
        RegexMatch match = RegexTypeMatching.TryMatch(messageText, regexTypes);
        DateType date = match.ConvertToDateType();
        Result<DateTime> dateTime = date.GetDateTime();
        if (!dateTime.IsSuccess)
        {
            await dateTime.Error.Send(client, update);
            update.ClearStepUserHandler();
            return;
        }

        CreateReportTaskDto dto = new CreateReportTaskDto(messageText, dateTime);
        CreateReportTaskRequest request = new CreateReportTaskRequest(dto);
        Result<ReportTask> createdTask = await _dispatcher.Dispatch<
            CreateReportTaskRequest,
            ReportTask
        >(request);
        if (!createdTask.IsSuccess)
        {
            await createdTask.Error.Send(client, update);
            update.ClearStepUserHandler();
            return;
        }

        update.ClearStepUserHandler();
        string reply = $"""
            Задача:

            {createdTask.Value.Message}

            создана на:

            {dateTime.Value.ToString("HH:mm dd.MM.yyyy")}
            """;
        await Message.Send(client, update, reply);
    }
}
