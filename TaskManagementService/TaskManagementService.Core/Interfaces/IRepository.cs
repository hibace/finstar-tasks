using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementService.Domain.Common;

namespace TaskManagementService.Core.Interfaces
{
    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public interface IRepository<T> where T : AuditableEntity
    {
        /// <summary>
        /// Получение сущности по идентификатору
        /// </summary>
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление сущности
        /// </summary>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление сущности
        /// </summary>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление сущности
        /// </summary>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
} 