using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация сущности TaskEntity
    /// </summary>
    public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        /// <summary>
        /// Настраивает конфигурацию сущности TaskEntity
        /// </summary>
        /// <param name="builder">Строитель конфигурации</param>
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
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
        }
    }
} 