using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask.Decorators;

public sealed class CreateReportTaskExceptionDecorator(
    ICommandHandler<CreateReportTaskCommand, ReportTask> handler,
    Serilog.ILogger logger
) : ICommandHandler<CreateReportTaskCommand, ReportTask>
{
    private readonly ICommandHandler<CreateReportTaskCommand, ReportTask> _handler = handler;
    private readonly Serilog.ILogger _logger = logger;

    public async Task<Result<ReportTask>> Handle(CreateReportTaskCommand command)
    {
        try
        {
            Result<ReportTask> result = await _handler.Handle(command);
            _logger.Information(
                "{Context} finished without exceptions.",
                nameof(CreateReportTaskCommand)
            );
            return result;
        }
        catch (Exception ex)
        {
            _logger.Fatal(
                "{Context} finished with exception. {Ex}",
                nameof(CreateReportTaskCommand),
                ex.Message
            );
            return new Error("Задача не создалась. Внутренняя ошибка приложения.");
        }
    }
}
