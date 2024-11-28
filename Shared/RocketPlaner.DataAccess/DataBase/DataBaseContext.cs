using Microsoft.EntityFrameworkCore;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;
using RocketPlaner.Core.models.Users;
using RocketPlaner.DataAccess.DataBase.Config;

namespace RocketPlaner.DataAccess.DataBase;

internal sealed class DataBaseContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RocketTask> Tasks { get; set; } = null!;
    public DbSet<RocketTaskDestination> Destinations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Data Source=Database.db");

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder
            .ApplyConfiguration(new UsersConfig())
            .ApplyConfiguration(new TasksConfig())
            .ApplyConfiguration(new DestinatonConfig());
}
