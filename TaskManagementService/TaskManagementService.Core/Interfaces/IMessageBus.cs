using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Core.Interfaces
{
    /// <summary>
    /// Определяет интерфейс для работы с шиной сообщений
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Публикует событие создания задачи
        /// </summary>
        /// <param name="task">Созданная задача</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        Task PublishTaskCreatedEvent(TaskEntity task);

        /// <summary>
        /// Публикует событие обновления задачи
        /// </summary>
        /// <param name="task">Обновленная задача</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        Task PublishTaskUpdatedEvent(TaskEntity task);

        /// <summary>
        /// Публикует событие удаления задачи
        /// </summary>
        /// <param name="taskId">Идентификатор удаленной задачи</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        Task PublishTaskDeletedEvent(Guid taskId);
    }
} 