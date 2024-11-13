using System;
using System.Windows.Input;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public class RemoveTaskForUsersCommand:ICommand<RocketTask>
{
    public long TelegrammID{get;init;}
    public string Title{get;init;}

    public RemoveTaskForUsersCommand (long telegrammID,string title)
    {
        this.TelegrammID = telegrammID;
        this.Title = title;

    }


}
