using Telegram.Bot;

namespace ReportTaskPlanner.TelegramBot.BackgroundServices;

public sealed class MessageNotifier : BackgroundService
{
    private readonly IServiceScopeFactory _factory;

    public MessageNotifier(IServiceScopeFactory factory) => _factory = factory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        ITelegramBotClient client = provider.GetRequiredService<ITelegramBotClient>();
    }
}
