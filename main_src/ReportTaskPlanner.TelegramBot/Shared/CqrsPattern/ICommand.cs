using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;

public interface ICommand<TCommandResult>;

public interface ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
{
    public Task<Result<TCommandResult>> Handle(TCommand command);
}
