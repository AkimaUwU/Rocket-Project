using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.DataAccess.DataBase.Config;

internal sealed class UsersConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .HasMany(u => u.Tasks)
            .WithOne(t => t.Owner)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ComplexProperty(
            u => u.TelegramId,
            cpb =>
            {
                cpb.Property(tid => tid.TelegramId);
            }
        );
    }
}
