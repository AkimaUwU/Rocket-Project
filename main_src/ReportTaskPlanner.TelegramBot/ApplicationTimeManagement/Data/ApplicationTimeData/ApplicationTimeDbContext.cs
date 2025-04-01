using Microsoft.EntityFrameworkCore;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData.EntityConfigurations;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Data;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;

public sealed class ApplicationTimeDbContext : CustomDbContext
{
    public DbSet<ApplicationTime> ApplicationTime { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Filename=ApplicationTimeDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new ApplicationTimeEntityConfiguration());
}
