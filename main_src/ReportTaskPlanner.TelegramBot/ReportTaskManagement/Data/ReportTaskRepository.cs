using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

public sealed class ReportTaskRepository(ReportTaskDbContext context)
{
    private readonly ReportTaskDbContext _context = context;

    public async Task Save(ReportTask task)
    {
        await _context.Collection.InsertAsync(task);
        await _context.Collection.EnsureIndexAsync(x => x.Id);
        await _context.Collection.EnsureIndexAsync(x => x.Schedule.TimeToNotify);
    }

    public async Task<IEnumerable<ReportTask>> GetPendingTasks(ApplicationTime time)
    {
        long seconds = time.TimeStamp;
        IEnumerable<ReportTask> tasks = await _context.Collection.FindAsync(x =>
            x.Schedule.TimeToNotify <= seconds
        );
        return tasks;
    }

    public async Task RemoveTasksByAppTime(ApplicationTime time)
    {
        long seconds = time.TimeStamp;
        await _context.Collection.DeleteManyAsync(x => x.Schedule.TimeToNotify <= seconds);
    }

    public async Task Remove(ReportTask task) => await _context.Collection.DeleteAsync(task.Id);
}
