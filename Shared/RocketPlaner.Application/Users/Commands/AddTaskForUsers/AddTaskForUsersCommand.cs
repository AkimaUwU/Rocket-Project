using System;
using System.Diagnostics.Contracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public class AddTaskForUsersCommand: ICommand<RocketTask>
{
    public string Messege{get;init;}
    public long IdUser{get;init;}
    public string TypeTask{get;init;}
    public DateTime NotifyDate{get;init;}
    public string TitleTask{get;init;}

    public string [] Destination {get;init;}

    public AddTaskForUsersCommand (string messege, long idUser, string typeTask, DateTime notifyDate,string title,params string [] destination)
    {
        this.Messege=messege;
        this.IdUser=idUser;
        this.TypeTask=typeTask;
        this.NotifyDate=notifyDate;
        this.TitleTask=title;
        this.Destination = destination;
    }
}
