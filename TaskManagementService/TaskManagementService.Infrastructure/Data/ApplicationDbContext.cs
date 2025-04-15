using Microsoft.EntityFrameworkCore;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Infrastructure.Configurations;

namespace TaskManagementService.Infrastructure.Data
{
    /// <summary>
    /// Контекст базы данных приложения
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
        }
    }
} 