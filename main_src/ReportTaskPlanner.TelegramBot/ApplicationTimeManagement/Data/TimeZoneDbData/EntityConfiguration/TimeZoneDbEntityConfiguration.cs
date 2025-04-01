using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData.EntityConfiguration;

public sealed class TimeZoneDbEntityConfiguration : IEntityTypeConfiguration<TimeZoneDbOptions>
{
    public void Configure(EntityTypeBuilder<TimeZoneDbOptions> builder)
    {
        builder.ToTable("Time_Zone_Db_Options");
        builder.HasKey(tz => tz.Token).HasName("token");
    }
}
