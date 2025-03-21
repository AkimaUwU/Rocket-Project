using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions;

public sealed record SetTimeZoneDbOptionsCommand(string Token) : ICommand<TimeZoneDbOptions>;

public sealed class SetTimeZoneDbOptionsCommandHandler
    : ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>
{
    private readonly TimeZoneDbRepository _repository;

    public SetTimeZoneDbOptionsCommandHandler(TimeZoneDbRepository repository) =>
        _repository = repository;

    public async Task<Result<TimeZoneDbOptions>> Handle(SetTimeZoneDbOptionsCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Token))
            return new Error("Токен был пустым.");
        TimeZoneDbOptions options = new TimeZoneDbOptions(command.Token);
        await _repository.Update(options);
        return options;
    }
}
