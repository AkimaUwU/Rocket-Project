using System;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand: ICommand<User>
{
    public long telegrammId{get;init;}
    public RegisterUserCommand (long TelegrammId)
    {
        telegrammId=TelegrammId;
    }
}
