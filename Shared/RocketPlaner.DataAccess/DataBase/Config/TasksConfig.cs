using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.application.Contracts.DataBaseContracts;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class TasksConfig : IEntityTypeConfiguration<TaskDataBaseTable>
{
    public void Configure(EntityTypeBuilder<TaskDataBaseTable> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t=>t.CreateDate).IsRequired();
        builder.Property(t=>t.NotifyDate).IsRequired();
        builder.Property(t=>t.Type).IsRequired();
        builder.Property(t=>t.Messege).IsRequired();
        builder.Property(t=>t.Title).IsRequired();
        builder.HasIndex(t => t.Title);
        builder.HasMany(t=>t.Destinations).WithOne(j=>j.Task).HasForeignKey(t=>t.TaskId);
        
    }
}
