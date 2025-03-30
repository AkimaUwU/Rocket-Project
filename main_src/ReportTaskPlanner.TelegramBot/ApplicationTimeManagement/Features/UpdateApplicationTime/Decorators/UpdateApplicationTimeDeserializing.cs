using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeserializeTimeZoneDbJson;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime.Decorators;

public sealed class UpdateApplicationTimeDeserializing
    : ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>
{
    private readonly UpdateApplicationTimeSharedContext _context;
    private readonly ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> _handler;

    public UpdateApplicationTimeDeserializing(
        ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> handler,
        UpdateApplicationTimeSharedContext context
    )
    {
        _handler = handler;
        _context = context;
    }

    public async Task<Result<ApplicationTime>> Handle(UpdateApplicationTimeCommand command)
    {
        string json = _context.TimeZoneDbJson.Value;
        TimeZoneDbJsonDeserializer deserializer = new(json);
        Result<ApplicationTime[]> deserialized = deserializer.Deserialize();
        if (deserialized.IsFailure)
            _context.SetError(deserialized.Error);

        ApplicationTime updated = deserialized.Value[0];
        _context.Deserialized = Option<ApplicationTime>.Some(updated);
        return await _handler.ExecuteNextWithErrorChecking(command, _context);
    }
}
