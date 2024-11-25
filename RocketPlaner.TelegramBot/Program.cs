using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Core.models.Users;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;
using RocketPlaner.TelegramBot;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddScoped<IUsersDataBase, UsersDatabase>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

builder.Services.AddTransient<
    ICommandHandler<RegisterUserCommand, User>,
    RegisterUserCommandHandler
>();

var host = builder.Build();
host.Run();
