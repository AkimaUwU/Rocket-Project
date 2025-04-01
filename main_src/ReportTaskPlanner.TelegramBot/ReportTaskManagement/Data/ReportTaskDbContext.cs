using Microsoft.EntityFrameworkCore;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data.EntityConfiguration;
using ReportTaskPlanner.TelegramBot.Shared.Data;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

public sealed class ReportTaskDbContext : CustomDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Filename=ReportTasksDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new ReportTaskEntityConfiguration());
}
