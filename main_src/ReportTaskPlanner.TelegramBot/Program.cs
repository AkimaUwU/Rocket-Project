using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using ReportTaskPlanner.TelegramBot;
using ReportTaskPlanner.TelegramBot.Configuration;

if (!Directory.Exists(ConfigurationVariables.ConfigurationFolder))
    Directory.CreateDirectory(ConfigurationVariables.ConfigurationFolder);

if (!File.Exists(ConfigurationVariables.TelegramBotTokenConfig))
    TgBotOptionsResolver.ResolveTgOptions();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddBotHandlers();
builder.Services.AddSingletonBotHandlers();
builder.Services.AddScopedBotHandlers();
builder.Services.AddTransientBotHandlers();

var host = builder.Build();

var timeZones = TimeZoneInfo.GetSystemTimeZones();

var localTimeZone = TimeZoneInfo.Local;
Console.WriteLine($"Time Zone ID: {localTimeZone.Id} / Time Zone Display Name: {localTimeZone.DisplayName}");

using (HttpClient client = new HttpClient())
{
    HttpResponseMessage responseMessage = await client.GetAsync("https://www.timeanddate.com/time/zone/russia");
    string response = await responseMessage.Content.ReadAsStringAsync();
    int a = 0;
}

foreach (var timeZone in timeZones)
{
    Console.WriteLine($"Time Zone ID: {timeZone.Id} / Time Zone Display Name: {timeZone.DisplayName}");
}

TgBotOptions options = TgBotOptionsResolver.LoadTgBotOptions();
IServiceProvider provider = host.Services.GetRequiredService<IServiceProvider>();
var botInstance = new PRBotBuilder(options.Token).SetClearUpdatesOnStart(true).SetServiceProvider(provider).Build();
await botInstance.Start();
Console.WriteLine("Bot started");
host.Run();