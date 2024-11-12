using System;
using System.Data.Common;
using RocketPlaner.domain.models.RocketTasks;
using RocketPlaner.domain.models.Users;

namespace RocketPlaner.application.Contracts.DataBaseContracts;

public interface IUsersDataBase
{
    Task AddUser(User user);
    Task RemoveUser(User user);
    Task GetUser(long TelegramId);


    

}
 
public class UserDataBaseTable
{
    public Guid Id;
    public long TelegramId;
    public List <TaskDataBaseTable> Tasks;
}

public class TaskDataBaseTable
{
    public Guid Id;
    public Guid OwnerId;
    public UserDataBaseTable Owner;
    public DateTime CreateDate;
    public DateTime NotifyDate;
    public string Type;
    public string Messege;
    public string Title;
    public List <DestinationDatBaseTable> Destinations;
    
}

public class DestinationDatBaseTable
{
    public Guid Id;
    public string ChatId;
    public TaskDataBaseTable Task;
    public Guid TaskId;
}
