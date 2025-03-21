using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
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
    private readonly ApplicationTimeRepository _repository;
    private readonly UpdateApplicationTimeSharedContext _context;

    public UpdateApplicationTimeCommandHandler(
        ApplicationTimeRepository repository,
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
