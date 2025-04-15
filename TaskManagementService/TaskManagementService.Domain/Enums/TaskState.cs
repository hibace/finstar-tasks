namespace TaskManagementService.Domain.Enums
{
    /// <summary>
    /// Статус задачи
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// Создана
        /// </summary>
        Created = 0,

        /// <summary>
        /// В работе
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// На проверке
        /// </summary>
        InReview = 2,

        /// <summary>
        /// Завершена
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Отменена
        /// </summary>
        Cancelled = 4
    }
} 