using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users;

public static class UserErrors
{
    public static Error UserNotFound => new Error("Пользователь не найден");
}