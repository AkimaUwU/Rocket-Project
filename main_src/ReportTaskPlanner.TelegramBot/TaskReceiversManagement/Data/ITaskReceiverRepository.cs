using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

public interface ITaskReceiverRepository
{
    Task<Result> Save(TaskReceiver receiver);
    Task<TaskReceiver[]> GetAll();
    Task<Result> Remove(long id);
}
