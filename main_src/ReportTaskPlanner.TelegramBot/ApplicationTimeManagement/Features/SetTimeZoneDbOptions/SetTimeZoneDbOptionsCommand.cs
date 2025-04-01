using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions;

public sealed record SetTimeZoneDbOptionsCommand(string Token) : ICommand<TimeZoneDbOptions>;

public sealed class SetTimeZoneDbOptionsCommandHandler
    : ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>
{
    private readonly ITimeZoneDbRepository _repository;

    public SetTimeZoneDbOptionsCommandHandler(ITimeZoneDbRepository repository) =>
        _repository = repository;

    public async Task<Result<TimeZoneDbOptions>> Handle(SetTimeZoneDbOptionsCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Token))
            return new Error("Токен был пустым.");
        TimeZoneDbOptions options = new TimeZoneDbOptions(command.Token);
        Result saving = await _repository.Save(options);
        return saving.IsSuccess ? options : saving.Error;
    }
}
