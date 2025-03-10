using FluentValidation;
using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.ReportTaskManagement.CreateReportTaskUseCase;

public sealed class CreateReportTaskDtoValidator : AbstractValidator<CreateReportTaskDto>
{
    public CreateReportTaskDtoValidator()
    {
        RuleFor(x => x)
            .Custom(
                (dto, context) =>
                {
                    Result<ReportTask> result = ReportTask.Create(dto.Message, dto.WhenToFire);
                    if (!result.IsSuccess)
                    {
                        context.AddFailure(result.Error.Message);
                        return;
                    }
                }
            );
    }
}
