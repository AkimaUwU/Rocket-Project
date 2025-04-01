using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateTimeZoneDbToken;

public sealed record UpdateTimeZoneDbTokenCommand(string Token) : ICommand<bool>;

public sealed class UpdateTimeZoneDbTokenCommandHandler(ITimeZoneDbRepository repository)
    : ICommandHandler<UpdateTimeZoneDbTokenCommand, bool>
{
    private readonly ITimeZoneDbRepository _repository = repository;

    public async Task<Result<bool>> Handle(UpdateTimeZoneDbTokenCommand command)
    {
        Result result = await _repository.Update(new TimeZoneDbOptions(command.Token));
        return result.IsSuccess ? true : result.Error;
    }
}
