using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace TgBotPlannerTests.Models.Tests;

public abstract class BaseRecognitionTest
{
    protected static async Task<ApplicationTime> GetCurrentTime(IServiceScopeFactory factory)
    {
        IServiceScope scope = factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> handler =
            provider.GetRequiredService<
                IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
            >();
        GetUpdatedApplicationTimeQuery query = new();
        Option<ApplicationTime> time = await handler.Handle(query);
        return time.Value;
    }
}
