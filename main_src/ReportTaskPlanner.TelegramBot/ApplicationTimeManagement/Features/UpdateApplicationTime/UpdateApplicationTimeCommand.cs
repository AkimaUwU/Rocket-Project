using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;

public sealed record UpdateApplicationTimeCommand(
    ApplicationTime Current,
    TimeZoneDbOptions Options
) : ICommand<ApplicationTime>;

public sealed class UpdateApplicationTimeCommandHandler
    : ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>
{
    private readonly IApplicationTimeRepository _repository;
    private readonly UpdateApplicationTimeSharedContext _context;

    public UpdateApplicationTimeCommandHandler(
        IApplicationTimeRepository repository,
        UpdateApplicationTimeSharedContext context
    )
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Result<ApplicationTime>> Handle(UpdateApplicationTimeCommand command)
    {
        ApplicationTime updated = _context.Deserialized.Value;
        await _repository.Update(updated);
        return updated;
    }
}
