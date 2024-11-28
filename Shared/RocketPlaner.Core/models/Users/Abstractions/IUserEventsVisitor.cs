using RocketPlaner.Core.models.Users.Events;

namespace RocketPlaner.Core.models.Users.Abstractions;

public interface IUserEventsVisitor
{
    Task Visit(UserCreated userCreated);
    Task Visit(UserAddedTask userAddedTask);
    Task Visit(UserRemovedTask userRemovedTask);
    Task Finish();
}
