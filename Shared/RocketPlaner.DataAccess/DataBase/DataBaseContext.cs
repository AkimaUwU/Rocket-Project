using Microsoft.EntityFrameworkCore;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.DataAccess.DataBase.Config;

namespace RocketPlaner.DataAccess.DataBase;

internal sealed class DataBaseContext : DbContext
{
    public DbSet<UsersDao> Users { get; set; } = null!;
    public DbSet<TasksDao> Tasks { get; set; } = null!;
    public DbSet<DestinationsDao> Destinations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Data Source=Database.db");

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder
            .ApplyConfiguration(new UsersConfig())
            .ApplyConfiguration(new TasksConfig())
            .ApplyConfiguration(new DestinatonConfig());
}
