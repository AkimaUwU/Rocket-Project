using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using ReportTaskPlanner.TelegramBot;
using ReportTaskPlanner.TelegramBot.Configuration;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using Serilog;
using Telegram.Bot;

if (!Directory.Exists(ConfigurationVariables.ConfigurationFolder))
    Directory.CreateDirectory(ConfigurationVariables.ConfigurationFolder);

if (!File.Exists(ConfigurationVariables.TelegramBotTokenConfigPath))
    TgBotOptionsResolver.ResolveTgOptions();

TgBotOptions options = TgBotOptionsResolver.LoadTgBotOptions();
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
Serilog.ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Services.AddSingleton(logger);
builder.Services.InjectAllServices();
builder.Services.AddScopedBotHandlers();
builder.Services.AddSingleton<Worker>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<TelegramBotClient>(_ =>
{
    TelegramBotClient client = new(options.Token);
    return client;
});

IHost host = builder.Build();
IServiceProvider provider = host.Services.GetRequiredService<IServiceProvider>();
PRBotBase botInstance = new PRBotBuilder(options.Token)
    .SetClearUpdatesOnStart(true)
    .SetServiceProvider(provider)
    .Build();

await botInstance.Start();
Console.WriteLine("Bot started");
host.Run();
