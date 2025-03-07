using FluentValidation.Results;
using ReportTaskPlanner.Utilities.ResultPattern;
using Serilog;

namespace ReportTaskPlanner.UseCases.Extensions;

public static class ErrorExtensions
{
    public static Error LogErrorAndReturn(this Error error, ILogger logger, string context)
    {
        if (error == Error.None())
            throw new ApplicationException("Error is none");
        logger.Error("{Context}. Error: {Message}", context, error.Message);
        return error;
    }

    public static Error LogErrorAndReturn(
        this ValidationResult validation,
        ILogger logger,
        string context
    )
    {
        if (validation.IsValid)
            throw new ApplicationException("Validation is valid");
        if (validation.Errors.Count == 0)
            throw new ApplicationException("Failed validation has no errors");

        IEnumerable<string> errors = validation.Errors.Select(err => err.ErrorMessage);
        string errorsMessage = string.Join(Environment.NewLine, errors);
        return new Error(errorsMessage).LogErrorAndReturn(logger, context);
    }
}
