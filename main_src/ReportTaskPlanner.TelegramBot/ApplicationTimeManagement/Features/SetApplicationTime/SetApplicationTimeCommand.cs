using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime;

public sealed record SetApplicationTimeCommand(ApplicationTime Time) : ICommand<ApplicationTime>;

public sealed class SetApplicationTimeCommandHandler
    : ICommandHandler<SetApplicationTimeCommand, ApplicationTime>
{
    private readonly IApplicationTimeRepository _repository;

    public SetApplicationTimeCommandHandler(IApplicationTimeRepository repository) =>
        _repository = repository;

    public async Task<Result<ApplicationTime>> Handle(SetApplicationTimeCommand command)
    {
        Result result = await _repository.Save(command.Time);
        return result.IsSuccess ? command.Time : result.Error;
    }
}
