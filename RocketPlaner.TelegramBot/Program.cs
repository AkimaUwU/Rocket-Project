using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.DependencyInjection;
using RocketPlaner.Application.RocketTasks.DependencyInjection;
using RocketPlaner.Application.Users.DependencyInjection;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;
using RocketPlaner.DataAccess.DependencyInjection;
using RocketPlaner.TelegramBot;
using RocketPlaner.TelegramBot.BotEndpoints.Abstractions;
using RocketPlaner.TelegramBot.BotEndpoints.CreateTaskEndpoint;
using RocketPlaner.TelegramBot.BotEndpoints.UserRegistrationEndpoint;
using RocketPlaner.TelegramBot.Configuration;
using RocketPlanner.TimeClassifier.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));
builder.Services.AddSingleton<BotSettings>();
builder.Services.AddBotHandlers();
builder.Services.AddApplicationCommonServices();
builder.Services.AddDataAccessServices();
builder.Services.AddUserApplicationServices();
builder.Services.AddRocketTasksServices();

builder.Services.AddSingleton<CreateTaskStateContainer>();
builder.Services.AddTimeClassification();

var token = builder.Configuration.GetSection("BotSettings").GetValue<string>("Token")!;
var host = builder.Build();
var serviceProvider = host.Services.GetRequiredService<IServiceProvider>();
var bot = new PRBotBuilder(token)
    .SetClearUpdatesOnStart(true)
    .SetServiceProvider(serviceProvider)
    .Build();
await bot.Start();
Console.WriteLine("Bot started");
host.Run();
