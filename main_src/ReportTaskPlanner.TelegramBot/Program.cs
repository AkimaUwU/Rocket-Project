using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using ReportTaskPlanner.DependencyInjection;
using ReportTaskPlanner.TelegramBot;
using ReportTaskPlanner.TelegramBot.Configuration;

if (!Directory.Exists(ConfigurationVariables.ConfigurationFolder))
    Directory.CreateDirectory(ConfigurationVariables.ConfigurationFolder);

if (!File.Exists(ConfigurationVariables.TelegramBotTokenConfigPath))
    TgBotOptionsResolver.ResolveTgOptions();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.InjectServices();
builder.Services.AddTransientBotHandlers();

var host = builder.Build();
TgBotOptions options = TgBotOptionsResolver.LoadTgBotOptions();
IServiceProvider provider = host.Services.GetRequiredService<IServiceProvider>();
var botInstance = new PRBotBuilder(options.Token).SetClearUpdatesOnStart(true).SetServiceProvider(provider).Build();
await botInstance.Start();
Console.WriteLine("Bot started");
host.Run();