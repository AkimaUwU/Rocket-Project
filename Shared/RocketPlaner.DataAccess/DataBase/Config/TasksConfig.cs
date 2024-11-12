using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.App.Contracts.DataBaseContracts;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class TasksConfig : IEntityTypeConfiguration<TasksDao>
{
    public void Configure(EntityTypeBuilder<TasksDao> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.CreateDate).IsRequired();
        builder.Property(t => t.NotifyDate).IsRequired();
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Message).IsRequired();
        builder.Property(t => t.Title).IsRequired();
        builder.HasIndex(t => t.Title);
        builder.HasMany(t => t.Destinations).WithOne(j => j.Task).HasForeignKey(t => t.TaskId);
    }
}
