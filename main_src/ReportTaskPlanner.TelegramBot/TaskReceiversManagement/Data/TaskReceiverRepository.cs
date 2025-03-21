using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

public sealed class TaskReceiverRepository(TaskReceiverDbContext context)
{
    private readonly TaskReceiverDbContext _context = context;

    public async Task Save(TaskReceiver receiver)
    {
        await _context.Collection.InsertAsync(receiver);
        await _context.Collection.EnsureIndexAsync(x => x.Id);
    }

    public async Task<bool> Contains(long id)
    {
        int count = await _context.Collection.CountAsync(x => x.Id == id);
        return count != 0;
    }

    public async Task<IEnumerable<TaskReceiver>> GetAll() =>
        await _context.Collection.FindAsync(x => true);

    public async Task<bool> Remove(long id) => await _context.Collection.DeleteAsync(id);
}
