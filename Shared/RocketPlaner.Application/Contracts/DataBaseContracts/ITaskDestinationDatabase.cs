using RocketPlaner.Core.models.RocketTaskDestinations;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public interface ITaskDestinationDatabase
{
    public Task RemoveDestination(RocketTaskDestination destination);
    public Task AddDestination(RocketTaskDestination destination);
}
