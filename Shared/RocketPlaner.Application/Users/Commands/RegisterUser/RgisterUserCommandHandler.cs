using System;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, User>
{
    private readonly IUsersDataBase userBase;
    public RegisterUserCommandHandler (IUsersDataBase userdb)
    {
        userBase=userdb;
    }
    public async Task<Result<User>> Handle(RegisterUserCommand command)
    {
       Result <User> user = User.Create(command.telegrammId);
       if (user.IsError)
       {
        return user.Error;
       }
        await userBase.AddUser(user.Value);
        return user;
    }
}
