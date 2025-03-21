using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime;

public sealed record SetApplicationTimeCommand(ApplicationTime Time) : ICommand<ApplicationTime>;

public sealed class SetApplicationTimeCommandHandler
    : ICommandHandler<SetApplicationTimeCommand, ApplicationTime>
{
    private readonly ApplicationTimeRepository _repository;

    public SetApplicationTimeCommandHandler(ApplicationTimeRepository repository) =>
        _repository = repository;

    public async Task<Result<ApplicationTime>> Handle(SetApplicationTimeCommand command)
    {
        await _repository.Update(command.Time);
        return command.Time;
    }
}
