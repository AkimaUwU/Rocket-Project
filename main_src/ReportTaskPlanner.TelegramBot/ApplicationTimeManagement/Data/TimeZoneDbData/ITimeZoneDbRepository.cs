using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;

public interface ITimeZoneDbRepository
{
    Task<Result> Save(TimeZoneDbOptions options);
    Task<Result<TimeZoneDbOptions>> Get();
    Task<Result> Update(TimeZoneDbOptions options);
    Task<Result> Delete(string token);
}
