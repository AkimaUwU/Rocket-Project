using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteTimeZoneDbToken;

public sealed record DeleteTimeZoneDbTokenCommand(string Token) : ICommand<bool>;

public sealed class DeleteTimeZoneDbTokenCommandHandler(ITimeZoneDbRepository repository)
    : ICommandHandler<DeleteTimeZoneDbTokenCommand, bool>
{
    private readonly ITimeZoneDbRepository _repository = repository;

    public async Task<Result<bool>> Handle(DeleteTimeZoneDbTokenCommand command)
    {
        Result result = await _repository.Delete(command.Token);
        return result.IsSuccess ? true : result.Error;
    }
}
