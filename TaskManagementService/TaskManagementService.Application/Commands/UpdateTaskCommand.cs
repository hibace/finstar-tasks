using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Core.Telemetry;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.Application.Commands
{
    /// <summary>
    /// Команда обновления задачи
    /// </summary>
    public class UpdateTaskCommand : IRequest<TaskDto>
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public Guid TaskId { get; }

        /// <summary>
        /// DTO для обновления задачи
        /// </summary>
        public UpdateTaskDto TaskDto { get; }

        public UpdateTaskCommand(Guid taskId, UpdateTaskDto taskDto)
        {
            TaskId = taskId;
            TaskDto = taskDto ?? throw new ArgumentNullException(nameof(taskDto));
        }
    }

    /// <summary>
    /// Обработчик команды обновления задачи
    /// </summary>
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UpdateTaskCommandHandler"/>
        /// </summary>
        /// <param name="taskRepository">Репозиторий задач</param>
        /// <param name="mapper">Объект для маппинга</param>
        /// <param name="messageBus">Брокер сообщений</param>
        public UpdateTaskCommandHandler(
            ITaskRepository taskRepository,
            IMapper mapper,
            IMessageBus messageBus)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        }

        /// <summary>
        /// Обрабатывает команду обновления задачи
        /// </summary>
        /// <param name="request">Команда обновления задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленная задача</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если задача не найдена</exception>
        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            using var activity = Tracing.StartActivity("UpdateTask");
            try
            {
                if (activity != null)
                {
                    Tracing.AddTag(activity, "task.id", request.TaskId);
                    Tracing.AddTag(activity, "task.title", request.TaskDto.Title);
                    Tracing.AddTag(activity, "task.state", request.TaskDto.State);
                }

                var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
                if (task == null)
                {
                    return null;
                }

                task.Update(request.TaskDto.Title, request.TaskDto.Description, request.TaskDto.State);
                task.Priority = request.TaskDto.Priority;
                task.DueDate = request.TaskDto.DueDate;
                task.UpdatedAt = DateTime.UtcNow;

                await _taskRepository.UpdateAsync(task, cancellationToken);

                if (activity != null)
                {
                    Tracing.AddEvent(activity, "TaskUpdated", 
                        ("TaskId", task.Id),
                        ("State", task.State));
                }

                await _messageBus.PublishTaskUpdatedEvent(task);

                return _mapper.Map<TaskDto>(task);
            }
            catch (Exception ex)
            {
                if (activity != null)
                {
                    Tracing.AddEvent(activity, "UpdateTaskError", 
                        ("Error", ex.Message));
                }
                throw;
            }
        }
    }
} 