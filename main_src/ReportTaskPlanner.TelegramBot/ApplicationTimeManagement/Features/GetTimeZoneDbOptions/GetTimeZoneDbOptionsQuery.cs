using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;

public sealed record GetTimeZoneDbOptionsQuery : IQuery<Option<TimeZoneDbOptions>>;

public sealed class GetTimeZoneDbOptionsQueryHandler
    : IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>>
{
    private readonly ITimeZoneDbRepository _repository;

    public GetTimeZoneDbOptionsQueryHandler(ITimeZoneDbRepository repository) =>
        _repository = repository;

    public async Task<Option<TimeZoneDbOptions>> Handle(GetTimeZoneDbOptionsQuery query)
    {
        Result<TimeZoneDbOptions> options = await _repository.Get();
        return options.IsFailure
            ? Option<TimeZoneDbOptions>.None()
            : Option<TimeZoneDbOptions>.Some(options);
    }
}
