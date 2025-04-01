using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;

public sealed record GetCurrentAppTimeQuery : IQuery<Option<ApplicationTime>>;

public sealed class GetCurrentAppTimeQueryHandler
    : IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>>
{
    private readonly IApplicationTimeRepository _repository;

    public GetCurrentAppTimeQueryHandler(IApplicationTimeRepository repository) =>
        _repository = repository;

    public async Task<Option<ApplicationTime>> Handle(GetCurrentAppTimeQuery query)
    {
        Result<ApplicationTime> time = await _repository.Get();
        return time.IsFailure ? Option<ApplicationTime>.None() : Option<ApplicationTime>.Some(time);
    }
}
