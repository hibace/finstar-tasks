using TaskManagementService.Domain.Common;
using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Domain.Entities
{
    /// <summary>
    /// Сущность задачи
    /// </summary>
    public class TaskEntity : AuditableEntity
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public TaskState State { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Срок выполнения задачи
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления задачи
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        private TaskEntity() { }

        public TaskEntity(string title, string description)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            State = TaskState.Created;
        }

        /// <summary>
        /// Обновление задачи
        /// </summary>
        public void Update(string title, string description, TaskState newState)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            State = newState;
            LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Изменение статуса задачи
        /// </summary>
        public void ChangeState(TaskState newState)
        {
            State = newState;
            LastModified = DateTime.UtcNow;
        }
    }
} 