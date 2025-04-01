using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data.EntityConfigurations;

public sealed class TaskReceiverEntityConfiguration : IEntityTypeConfiguration<TaskReceiver>
{
    public void Configure(EntityTypeBuilder<TaskReceiver> builder)
    {
        builder.ToTable("Task_Receivers");
        builder.HasKey(r => r.Id).HasName("receiver_id");
        builder.Property(r => r.IsEnabled).HasColumnName("is_enabled");
    }
}
