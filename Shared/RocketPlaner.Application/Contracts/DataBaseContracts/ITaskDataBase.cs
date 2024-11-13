using System;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public interface ITaskDataBase
{
    Task AddTask(RocketTask task);
    Task Remove(RocketTask task);
    Task Update (RocketTask task);
}
