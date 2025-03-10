using FluentValidation.Results;
using ReportTaskPlanner.UseCases.Abstractions.Handlers;
using ReportTaskPlanner.UseCases.Extensions;
using ReportTaskPlanner.Utilities.ResultPattern;
using Serilog;

namespace ReportTaskPlanner.UseCases.ReportTaskManagement.DeleteReportTaskUseCase;

public record DeleteReportTaskRequest(string? Id = null, string? Message = null) : IRequest;

public record DeleteReportTaskRequestHandler : IHandler<DeleteReportTaskRequest>
{
    private readonly ILogger _logger;
    private readonly RemoveReportTaskRequestValidator _validator;
    private readonly RemoveReportTaskFromPersistance _remove;

    public DeleteReportTaskRequestHandler(
        ILogger logger,
        RemoveReportTaskRequestValidator validator,
        RemoveReportTaskFromPersistance remove
    ) => (_logger, _validator, _remove) = (logger, validator, remove);

    public async Task<Result> Handle(
        DeleteReportTaskRequest request,
        CancellationToken ct = default
    )
    {
        ValidationResult validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return validation.LogErrorAndReturn(_logger, nameof(DeleteReportTaskRequestHandler));

        Result removing = await _remove.Remove(request);
        if (!removing.IsSuccess)
            return removing.Error.LogErrorAndReturn(
                _logger,
                nameof(DeleteReportTaskRequestHandler)
            );

        return Result.Success();
    }
}
