using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.TimeRecognition;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask.Decorators;

public sealed class CreateReportTaskRecognizeTimeDecorator(
    CreateReportTaskContext context,
    ICommandHandler<CreateReportTaskCommand, ReportTask> handler
) : ICommandHandler<CreateReportTaskCommand, ReportTask>
{
    private readonly CreateReportTaskContext _context = context;
    private readonly ICommandHandler<CreateReportTaskCommand, ReportTask> _handler = handler;

    public async Task<Result<ReportTask>> Handle(CreateReportTaskCommand command)
    {
        string input = command.TextPayload;
        try
        {
            Option<TimeRecognitionResult> recognition = TimeRecognizer.TryRecognize(input);
            if (!recognition.HasValue)
                return new Error("Не указано время в сообщении (формат: ЧЧ:ММ или ЧЧ ММ)");
            _context.TimeRecognitionResult = Option<TimeRecognitionResult>.Some(recognition);
            return await _handler.Handle(command);
        }
        catch
        {
            return new Error("Не указано время в сообщении (формат: ЧЧ:ММ или ЧЧ ММ)");
        }
    }
}
