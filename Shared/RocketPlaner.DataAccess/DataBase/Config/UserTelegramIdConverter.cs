using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RocketPlaner.Core.models.Users.ValueObjects;

namespace RocketPlaner.DataAccess.DataBase.Config;


// internal sealed class UserTelegramIdConverter : ValueConverter<UserTelegramId, long>
// {
//     public UserTelegramIdConverter()
//         : base(id => id.TelegramId, value => UserTelegramId.Create(value).Value, false) { }
// }
