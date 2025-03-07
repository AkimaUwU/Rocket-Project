using FluentValidation;

namespace ReportTaskPlanner.UseCases.DeleteReportTaskUseCase;

public sealed class RemoveReportTaskRequestValidator : AbstractValidator<DeleteReportTaskRequest>
{
    public RemoveReportTaskRequestValidator()
    {
        RuleFor(req => req)
            .Custom(
                (request, context) =>
                {
                    string? id = request.Id;
                    string? message = request.Message;
                    if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(message))
                    {
                        context.AddFailure(
                            "Нет параметра для удаления задачи (ИД или текст сообщения)."
                        );
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(message))
                    {
                        context.AddFailure(
                            "Параметр удаления (ИД или текст сообщения) должен быть один."
                        );
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        bool canParseGuid = Guid.TryParse(id, out Guid parsedId);
                        if (!canParseGuid)
                            context.AddFailure("Некорректный ID.");
                    }
                }
            );
    }
}
