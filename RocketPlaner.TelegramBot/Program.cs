using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.DependencyInjection;
using RocketPlaner.Application.RocketTasks.DependencyInjection;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Application.Users.DependencyInjection;
using RocketPlaner.Core.models.Users;
using RocketPlaner.DataAccess.DependencyInjection;
using RocketPlaner.TelegramBot;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder
    .Services.AddApplicationCommonServices()
    .AddDataAccessServices()
    .AddUserApplicationServices()
    .AddRocketTasksServices();

builder.Services.AddTransient<
    ICommandHandler<RegisterUserCommand, User>,
    RegisterUserCommandHandler
>();

var host = builder.Build();
host.Run();
