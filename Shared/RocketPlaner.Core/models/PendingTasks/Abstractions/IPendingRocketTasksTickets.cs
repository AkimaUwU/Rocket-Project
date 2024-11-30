namespace RocketPlaner.Core.models.PendingTasks.Abstractions;

public interface IPendingRocketTasksTickets
{
    Task AddTicket(PendingRocketTask task);
    Task RemoveTicket(PendingRocketTask task);
    Task UpdateTicket(PendingRocketTask task);
    Task<IEnumerable<PendingRocketTask>> FetchPendingTickets();
}

public interface IPendingRocketTaskChats
{
    Task AddChat(PendingRocketTaskChat chat);
    Task RemoveChat(PendingRocketTaskChat chat);
}
