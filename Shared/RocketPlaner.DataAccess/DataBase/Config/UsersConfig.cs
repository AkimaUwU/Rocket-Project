using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.ValueObjects;

namespace RocketPlaner.DataAccess.DataBase.Config;

internal sealed class UsersConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.TelegramId);

        builder
            .Property(u => u.TelegramId)
            .HasConversion(id => id.TelegramId, value => UserTelegramId.Create(value).Value);

        builder
            .HasMany(u => u.Tasks)
            .WithOne(t => t.Owner)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
