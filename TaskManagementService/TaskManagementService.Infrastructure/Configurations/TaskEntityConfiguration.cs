using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности TaskEntity
    /// </summary>
    public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(t => t.State)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .IsRequired();

            // Индексы
            builder.HasIndex(t => t.State);
            builder.HasIndex(t => t.CreatedAt);
            builder.HasIndex(t => t.Priority);
        }
    }
} 