using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketPlaner.application.Contracts.DataBaseContracts;

namespace RocketPlaner.DataAccess.DataBase.Config;

public class DestinatonConfig : IEntityTypeConfiguration<DestinationDatBaseTable>
{
    public void Configure(EntityTypeBuilder<DestinationDatBaseTable> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t=>t.ChatId).IsRequired();
        builder.HasIndex(t => t.ChatId);
       
    }
}
