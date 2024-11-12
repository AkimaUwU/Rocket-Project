using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.App.Contracts.DataBaseContracts;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class DestinatonConfig : IEntityTypeConfiguration<DestinationsDao>
{
    public void Configure(EntityTypeBuilder<DestinationsDao> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.ChatId).IsRequired();
        builder.HasIndex(t => t.ChatId);
    }
}
