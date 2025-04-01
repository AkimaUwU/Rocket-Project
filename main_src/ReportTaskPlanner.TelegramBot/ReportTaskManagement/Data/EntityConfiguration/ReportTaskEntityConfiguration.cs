using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data.EntityConfiguration;

public sealed class ReportTaskEntityConfiguration : IEntityTypeConfiguration<ReportTask>
{
    public void Configure(EntityTypeBuilder<ReportTask> builder)
    {
        builder.ToTable("Report_Tasks");
        builder.HasKey(t => t.Id).HasName("task_id");
        builder.Property(t => t.Text).HasColumnName("task_text");
        builder.ComplexProperty<ReportTaskSchedule>(
            t => t.Schedule,
            cpb =>
            {
                cpb.Property(s => s.TimeCreated).HasColumnName("task_schedule_created_unix");
                cpb.Property(s => s.TimeToNotify).HasColumnName("task_schedule_notify_time");
                cpb.Property(s => s.IsPeriodic).HasColumnName("task_schedule_is_periodic");
            }
        );
    }
}
