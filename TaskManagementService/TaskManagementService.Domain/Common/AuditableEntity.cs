using System;

namespace TaskManagementService.Domain.Common
{
    /// <summary>
    /// Базовый класс для аудируемых сущностей
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime LastModified { get; protected set; }

        protected AuditableEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastModified = CreatedAt;
        }
    }
} 