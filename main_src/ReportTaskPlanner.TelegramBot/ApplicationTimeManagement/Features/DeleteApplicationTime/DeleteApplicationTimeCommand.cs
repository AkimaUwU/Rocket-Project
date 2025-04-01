using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteApplicationTime;

public sealed record DeleteApplicationTimeCommand(string ZoneName) : ICommand<bool>;

public sealed class DeleteApplicationTimeCommandHandler(IApplicationTimeRepository repository)
    : ICommandHandler<DeleteApplicationTimeCommand, bool>
{
    private readonly IApplicationTimeRepository _repository = repository;

    public async Task<Result<bool>> Handle(DeleteApplicationTimeCommand command)
    {
        Result result = await _repository.Delete(command.ZoneName);
        return result.IsSuccess ? true : result.Error;
    }
}
