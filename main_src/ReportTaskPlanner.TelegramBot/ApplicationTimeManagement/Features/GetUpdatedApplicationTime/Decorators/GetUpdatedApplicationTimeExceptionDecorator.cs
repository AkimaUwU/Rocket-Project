using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime.Decorators;

public sealed class GetUpdatedApplicationTimeExceptionDecorator(
    IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> handler,
    ILogger logger
) : IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
{
    private readonly IQueryHandler<
        GetUpdatedApplicationTimeQuery,
        Option<ApplicationTime>
    > _handler = handler;

    private readonly ILogger _logger = logger;

    public async Task<Option<ApplicationTime>> Handle(GetUpdatedApplicationTimeQuery query)
    {
        try
        {
            Option<ApplicationTime> result = await _handler.Handle(query);
            _logger.Information(
                "{Name} processed without exceptions.",
                nameof(GetUpdatedApplicationTimeQuery)
            );
            return result;
        }
        catch (Exception ex)
        {
            _logger.Fatal(
                "{Name} processed with exception: {Ex}",
                nameof(GetUpdatedApplicationTimeQuery),
                ex.Message
            );
            return Option<ApplicationTime>.None();
        }
    }
}
