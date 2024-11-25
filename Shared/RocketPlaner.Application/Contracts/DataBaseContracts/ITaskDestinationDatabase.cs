namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public interface ITaskDestinationDatabase
{
    public Task RemoveDestination(DestinationsDao destination);
    public Task AddDestination(DestinationsDao destination);
}
