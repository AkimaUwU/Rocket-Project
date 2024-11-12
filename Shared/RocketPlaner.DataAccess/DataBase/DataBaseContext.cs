using System;
using Microsoft.EntityFrameworkCore;
using RocketPlaner.application.Contracts.DataBaseContracts;
using RocketPlaner.DataAccess.DataBase.Config;
using RocketPlaner.domain.models.Users;

namespace RocketPlaner.DataAccess.DataBase;

internal sealed class DataBaseContext: DbContext
{
    public DbSet<UserDataBaseTable> Users { get; set; } = null!;
    public DbSet<TaskDataBaseTable> Tasks { get; set; } = null!;
    public DbSet<DestinationDatBaseTable> Destinations { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Database.db");
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UsersConfig());
        builder.ApplyConfiguration(new TasksConfig());
        builder.ApplyConfiguration(new DestinatonConfig());
    }
}


