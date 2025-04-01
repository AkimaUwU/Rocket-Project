using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

public interface IReportTaskRepository
{
    Task<Result> Save(ReportTask task);
    Task<ReportTask[]> GetPendingTasks(ApplicationTime time);
    Task<Result<int>> RemoveMany(IEnumerable<ReportTask> tasks);
    Task<Result<int>> UpdateMany(IEnumerable<ReportTask> tasks);
}
