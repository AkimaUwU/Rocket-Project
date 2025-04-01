using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData.EntityConfigurations;

public sealed class ApplicationTimeEntityConfiguration : IEntityTypeConfiguration<ApplicationTime>
{
    public void Configure(EntityTypeBuilder<ApplicationTime> builder)
    {
        builder.ToTable("Application_Time");
        builder.HasKey(t => t.ZoneName).HasName("time_zone_name");
        builder.Property(t => t.TimeStamp).HasColumnName("time_stamp");
        builder.Property(t => t.DisplayName).HasColumnName("display_name");
        builder.Property(t => t.DateTime).HasColumnName("date_time");
    }
}
