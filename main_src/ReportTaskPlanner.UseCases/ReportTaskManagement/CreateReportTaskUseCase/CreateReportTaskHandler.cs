using FluentValidation.Results;
using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.UseCases.Abstractions.Handlers;
using ReportTaskPlanner.UseCases.Extensions;
using ReportTaskPlanner.Utilities.ResultPattern;
using Serilog;

namespace ReportTaskPlanner.UseCases.ReportTaskManagement.CreateReportTaskUseCase;

public sealed record CreateReportTaskRequest(CreateReportTaskDto Dto) : IRequest<ReportTask>;

public sealed class CreateReportTaskHandler : IHandler<CreateReportTaskRequest, ReportTask>
{
    private readonly ILogger _logger;
    private readonly SaveReportTaskInPersistance _save;
    private readonly CreateReportTaskDtoValidator _validator;

    public CreateReportTaskHandler(
        ILogger logger,
        SaveReportTaskInPersistance save,
        CreateReportTaskDtoValidator validator
    )
    {
        _logger = logger;
        _save = save;
        _validator = validator;
    }

    public async Task<Result<ReportTask>> Handle(
        CreateReportTaskRequest request,
        CancellationToken ct = default
    )
    {
        CreateReportTaskDto dto = request.Dto;
        ValidationResult validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return validation.LogErrorAndReturn(_logger, nameof(CreateReportTaskRequest));

        ReportTask task = dto.Create();
        Result saving = await _save.Save(task);
        if (!saving.IsSuccess)
            return saving.Error.LogErrorAndReturn(_logger, nameof(CreateReportTaskRequest));

        return task;
    }
}
