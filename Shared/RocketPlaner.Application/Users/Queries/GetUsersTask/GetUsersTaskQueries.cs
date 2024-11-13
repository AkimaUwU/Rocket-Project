using System;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQueries:IQueries<IReadOnlyList<RocketTask>>
{
    public long TelegrammID { get; init;}
    public GetUsersTaskQueries(long telegrammID)
    {
        this.TelegrammID = telegrammID;
    
    }
}
