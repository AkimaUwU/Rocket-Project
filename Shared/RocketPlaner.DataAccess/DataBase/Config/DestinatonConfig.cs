using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.Core.models.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTaskDestinations.ValueObjects;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class DestinatonConfig : IEntityTypeConfiguration<RocketTaskDestination>
{
    public void Configure(EntityTypeBuilder<RocketTaskDestination> builder)
    {
        builder.HasKey(d => d.ChatId);

        builder
            .Property(d => d.ChatId)
            .HasConversion(id => id.ChatId, value => DestinationChatId.Create(value).Value);
    }
}
