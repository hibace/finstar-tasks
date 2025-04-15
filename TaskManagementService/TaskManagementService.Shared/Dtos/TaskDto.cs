using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Shared.Dtos
{
    /// <summary>
    /// DTO задачи
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public required string Description { get; set; }

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
    }

    /// <summary>
    /// DTO для создания задачи
    /// </summary>
    public class CreateTaskDto
    {
        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Срок выполнения задачи
        /// </summary>
        public DateTime? DueDate { get; set; }
    }

    /// <summary>
    /// DTO для обновления задачи
    /// </summary>
    public class UpdateTaskDto
    {
        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public required string Description { get; set; }

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
    }
} 