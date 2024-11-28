using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class DestinatonConfig : IEntityTypeConfiguration<RocketTaskDestination>
{
    public void Configure(EntityTypeBuilder<RocketTaskDestination> builder)
    {
        builder.HasKey(t => t.Id);
        builder.ComplexProperty(
            d => d.ChatId,
            cpb =>
            {
                cpb.Property(cid => cid.ChatId).IsRequired();
            }
        );
    }
}
