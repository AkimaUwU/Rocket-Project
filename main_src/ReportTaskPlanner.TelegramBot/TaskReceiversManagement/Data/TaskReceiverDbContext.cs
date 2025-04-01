using Microsoft.EntityFrameworkCore;
using ReportTaskPlanner.TelegramBot.Shared.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data.EntityConfigurations;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

public sealed class TaskReceiverDbContext : CustomDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Filename=TaskReceiversDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new TaskReceiverEntityConfiguration());
}
