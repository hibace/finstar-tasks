using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Core.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с задачами
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Получает задачу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача или null, если задача не найдена</returns>
        Task<TaskEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает список всех задач
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач</returns>
        Task<IReadOnlyList<TaskEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет новую задачу
        /// </summary>
        /// <param name="entity">Задача для добавления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Добавленная задача</returns>
        Task<TaskEntity> AddAsync(TaskEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет существующую задачу
        /// </summary>
        /// <param name="entity">Задача для обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленная задача</returns>
        Task UpdateAsync(TaskEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="entity">Задача для удаления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteAsync(TaskEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет все изменения в базе данных
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Количество измененных записей</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает список задач по состоянию
        /// </summary>
        /// <param name="state">Состояние задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач с указанным состоянием</returns>
        Task<IReadOnlyList<TaskEntity>> GetByStateAsync(TaskState state, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение задач, созданных после указанной даты
        /// </summary>
        Task<IReadOnlyList<TaskEntity>> GetCreatedAfterAsync(DateTime date, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение задач, измененных после указанной даты
        /// </summary>
        Task<IReadOnlyList<TaskEntity>> GetModifiedAfterAsync(DateTime date, CancellationToken cancellationToken = default);
    }
} 