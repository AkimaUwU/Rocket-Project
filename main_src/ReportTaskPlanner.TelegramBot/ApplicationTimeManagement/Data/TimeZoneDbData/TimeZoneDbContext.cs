using Microsoft.EntityFrameworkCore;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData.EntityConfiguration;
using ReportTaskPlanner.TelegramBot.Shared.Data;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;

public sealed class TimeZoneDbContext : CustomDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Filename=TimeZoneDbConfigDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new TimeZoneDbEntityConfiguration());
}
