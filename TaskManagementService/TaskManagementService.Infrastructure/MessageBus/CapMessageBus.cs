using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Infrastructure.MessageBus
{
    /// <summary>
    /// Реализация шины сообщений на основе CAP
    /// </summary>
    public class CapMessageBus : IMessageBus
    {
        private readonly ICapPublisher _capPublisher;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CapMessageBus"/>
        /// </summary>
        /// <param name="capPublisher">Издатель сообщений CAP</param>
        public CapMessageBus(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher ?? throw new ArgumentNullException(nameof(capPublisher));
        }

        /// <summary>
        /// Публикует событие создания задачи
        /// </summary>
        /// <param name="task">Созданная задача</param>
        public async Task PublishTaskCreatedEvent(TaskEntity task)
        {
            await _capPublisher.PublishAsync("task.created", task);
        }

        /// <summary>
        /// Публикует событие обновления задачи
        /// </summary>
        /// <param name="task">Обновленная задача</param>
        public async Task PublishTaskUpdatedEvent(TaskEntity task)
        {
            await _capPublisher.PublishAsync("task.updated", task);
        }

        /// <summary>
        /// Публикует событие удаления задачи
        /// </summary>
        /// <param name="taskId">Идентификатор удаленной задачи</param>
        public async Task PublishTaskDeletedEvent(Guid taskId)
        {
            await _capPublisher.PublishAsync("task.deleted", new { Id = taskId });
        }
    }
} 