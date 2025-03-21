using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;

public sealed class TimeZoneDbRepository(TimeZoneDbContext context)
{
    private readonly TimeZoneDbContext _context = context;

    public async Task<Result> Save(TimeZoneDbOptions options)
    {
        bool hasSomething = (await _context.Collection.FindAllAsync()).Any();
        if (hasSomething)
            return new Error("Конфигурация Time Zone провайдера может быть только одна.");

        await _context.Collection.InsertAsync(options);
        return Result.Success();
    }

    public async Task<Result<TimeZoneDbOptions>> Get()
    {
        TimeZoneDbOptions? options = (
            await _context.Collection.FindAsync(_ => true)
        ).FirstOrDefault();

        return options == null
            ? new Error("В приложении отсутствует конфигурация Time Zone провайдера.")
            : options;
    }

    public async Task Update(TimeZoneDbOptions options)
    {
        await _context.Collection.DeleteAllAsync();
        await Save(options);
    }
}
