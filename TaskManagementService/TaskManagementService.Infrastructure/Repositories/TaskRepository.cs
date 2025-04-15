using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Infrastructure.Data;

namespace TaskManagementService.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с задачами
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TaskRepository"/>
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Получает задачу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача или null, если задача не найдена</returns>
        public async Task<TaskEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// Получает список всех задач
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач</returns>
        public async Task<IReadOnlyList<TaskEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавляет новую задачу
        /// </summary>
        /// <param name="entity">Задача для добавления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Добавленная задача</returns>
        public async Task<TaskEntity> AddAsync(TaskEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Обновляет существующую задачу
        /// </summary>
        /// <param name="entity">Задача для обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленная задача</returns>
        public async Task UpdateAsync(TaskEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="entity">Задача для удаления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public async Task DeleteAsync(TaskEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Сохраняет все изменения в базе данных
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Количество измененных записей</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Получает список задач по состоянию
        /// </summary>
        /// <param name="state">Состояние задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач с указанным состоянием</returns>
        public async Task<IReadOnlyList<TaskEntity>> GetByStateAsync(TaskState state, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Where(t => t.State == state)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получает список задач, созданных после указанной даты
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач</returns>
        public async Task<IReadOnlyList<TaskEntity>> GetCreatedAfterAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Where(t => t.CreatedAt > date)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получает список задач, измененных после указанной даты
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач</returns>
        public async Task<IReadOnlyList<TaskEntity>> GetModifiedAfterAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Where(t => t.UpdatedAt > date)
                .ToListAsync(cancellationToken);
        }
    }
} 