using ReportTaskPlanner.TelegramBot.Shared.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;

public static class ICommandHandlerExtensions
{
    public static async Task<Result<TCommandResult>> ExecuteNextWithErrorChecking<
        TCommand,
        TCommandResult
    >(
        this ICommandHandler<TCommand, TCommandResult> handler,
        TCommand command,
        SharedContext context
    )
        where TCommand : ICommand<TCommandResult>
    {
        if (context.HasError)
            return context.Error;
        return await handler.Handle(command);
    }
}
