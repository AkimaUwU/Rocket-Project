using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;

public interface IApplicationTimeRepository
{
    Task<Result> Save(ApplicationTime time);
    Task<Result<ApplicationTime>> Get();
    Task<Result> Update(ApplicationTime time);
    Task<Result> Delete(string zoneName);
}
