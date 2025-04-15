namespace TaskManagementService.Infrastructure.Options
{
    /// <summary>
    /// Настройки RabbitMQ
    /// </summary>
    public class RabbitMQOptions
    {
        /// <summary>
        /// Имя хоста
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; } = "guest";

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = "guest";

        /// <summary>
        /// Имя обменника для событий создания задачи
        /// </summary>
        public string TaskCreatedExchange { get; set; } = "task.created";

        /// <summary>
        /// Имя обменника для событий обновления задачи
        /// </summary>
        public string TaskUpdatedExchange { get; set; } = "task.updated";

        /// <summary>
        /// Имя обменника для событий удаления задачи
        /// </summary>
        public string TaskDeletedExchange { get; set; } = "task.deleted";
    }
} 