using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask.Decorators;

public class CreateReportTaskLoggingDecorator(
    Serilog.ILogger logger,
    ICommandHandler<CreateReportTaskCommand, ReportTask> handler
) : ICommandHandler<CreateReportTaskCommand, ReportTask>
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly ICommandHandler<CreateReportTaskCommand, ReportTask> _handler = handler;

    public async Task<Result<ReportTask>> Handle(CreateReportTaskCommand command)
    {
        _logger.Information("{Context} invoked.", nameof(CreateReportTask));
        Result<ReportTask> result = await _handler.Handle(command);
        if (result.IsFailure)
            _logger.LogError(result.Error, nameof(CreateReportTask));
        else
            _logger.Information(
                "{Context} created report task",
                nameof(CreateReportTask),
                result.Value.ToString()
            );
        return result;
    }
}
