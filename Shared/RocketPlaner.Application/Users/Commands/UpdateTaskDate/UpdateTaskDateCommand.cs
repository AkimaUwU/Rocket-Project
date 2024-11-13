using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public class UpdateTaskDateCommand:ICommand<RocketTask>
{
    public long TelegrammID { get; init; }
    public string Title{ get; init; }
    public DateTime NewDate{ get; init; }

    public UpdateTaskDateCommand(long telegrammID, string title, DateTime newDateTime)
    {
        this.TelegrammID = telegrammID;
        this.Title = title;
        this.NewDate = newDateTime;
    }


}
