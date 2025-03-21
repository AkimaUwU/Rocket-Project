using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;

public sealed class ApplicationTimeRepository(ApplicationTimeDbContext context)
{
    private readonly ApplicationTimeDbContext _context = context;

    public async Task<Result> Save(ApplicationTime time)
    {
        int count = await _context.Collection.CountAsync();
        if (count > 0)
            return new Error("В приложении можеть быть только одна конфигурация Time Zone Db.");

        await _context.Collection.InsertAsync(time);
        return Result.Success();
    }

    public async Task<Result<ApplicationTime>> Get()
    {
        ApplicationTime? time = (await _context.Collection.FindAsync(_ => true)).FirstOrDefault();
        return time == null ? new Error("В приложении нет конфигурации времени.") : time;
    }

    public async Task Update(ApplicationTime time)
    {
        await _context.Collection.DeleteAllAsync();
        await Save(time);
    }
}
