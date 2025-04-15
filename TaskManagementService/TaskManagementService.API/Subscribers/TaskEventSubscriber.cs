using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using TaskManagementService.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace TaskManagementService.API.Subscribers
{
    /// <summary>
    /// Подписчик на события задач
    /// </summary>
    public class TaskEventSubscriber : ICapSubscribe
    {
        private readonly ILogger<TaskEventSubscriber> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TaskEventSubscriber"/>
        /// </summary>
        /// <param name="logger">Логгер</param>
        public TaskEventSubscriber(ILogger<TaskEventSubscriber> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Обработчик события создания задачи
        /// </summary>
        /// <param name="task">Созданная задача</param>
        [CapSubscribe("task.created")]
        public Task HandleTaskCreatedEvent(TaskEntity task)
        {
            var taskId = task.Id.ToString();
            _logger.Log(LogLevel.Information, "Получено событие создания задачи: {TaskId}", taskId);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Обработчик события обновления задачи
        /// </summary>
        /// <param name="task">Обновленная задача</param>
        [CapSubscribe("task.updated")]
        public Task HandleTaskUpdatedEvent(TaskEntity task)
        {
            var taskId = task.Id.ToString();
            _logger.Log(LogLevel.Information, "Получено событие обновления задачи: {TaskId}", taskId);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Обработчик события удаления задачи
        /// </summary>
        /// <param name="message">Сообщение с идентификатором удаленной задачи</param>
        [CapSubscribe("task.deleted")]
        public Task HandleTaskDeletedEvent(dynamic message)
        {
            var taskId = message.Id.ToString();
            _logger.Log(LogLevel.Information, $"Получено событие удаления задачи: {taskId}");
            return Task.CompletedTask;
        }
    }
} 