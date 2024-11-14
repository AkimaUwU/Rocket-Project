using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.Application.Contracts.DataBaseContracts;

namespace RocketPlaner.DataAccess.DataBase.Config;

internal sealed class UsersConfig : IEntityTypeConfiguration<UsersDao>
{
    public void Configure(EntityTypeBuilder<UsersDao> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.TelegramId).IsRequired();
        builder.HasIndex(t => t.TelegramId);
        builder.HasMany(t => t.Tasks).WithOne(j => j.Owner).HasForeignKey(t => t.OwnerId);
    }
}
