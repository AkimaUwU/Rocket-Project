using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Sqlite.Metadata.Internal;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class TasksConfig : IEntityTypeConfiguration<RocketTask>
{
    public void Configure(EntityTypeBuilder<RocketTask> builder)
    {
        builder.HasKey(t => t.Id).HasAnnotation(SqliteAnnotationNames.Autoincrement, true);

        builder
            .HasMany(t => t.Destinations)
            .WithOne(d => d.BelongsTo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ComplexProperty(
            t => t.Title,
            cpb =>
            {
                cpb.Property(tit => tit.Title).IsRequired();
            }
        );

        builder.ComplexProperty(
            t => t.Message,
            cpb =>
            {
                cpb.Property(m => m.Message).IsRequired();
            }
        );

        builder.ComplexProperty(
            t => t.Type,
            cpb =>
            {
                cpb.Property(ty => ty.Type).IsRequired();
            }
        );

        builder.ComplexProperty(
            t => t.FireDate,
            cpb =>
            {
                cpb.Property(fd => fd.FireDate).IsRequired();
            }
        );
    }
}
