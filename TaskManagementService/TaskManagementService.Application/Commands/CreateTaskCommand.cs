using AutoMapper;
using MediatR;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Core.Telemetry;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.Application.Commands
{
    /// <summary>
    /// Команда создания задачи
    /// </summary>
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        /// <summary>
        /// DTO для создания задачи
        /// </summary>
        public CreateTaskDto TaskDto { get; }

        public CreateTaskCommand(CreateTaskDto taskDto)
        {
            TaskDto = taskDto ?? throw new ArgumentNullException(nameof(taskDto));
        }
    }

    /// <summary>
    /// Обработчик команды создания задачи
    /// </summary>
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;

        public CreateTaskCommandHandler(
            ITaskRepository taskRepository,
            IMapper mapper,
            IMessageBus messageBus)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            using var activity = Tracing.StartActivity("CreateTask");
            try
            {
                if (activity != null)
                {
                    Tracing.AddTag(activity, "task.title", request.TaskDto.Title);
                    Tracing.AddTag(activity, "task.priority", request.TaskDto.Priority);
                }

                var task = new TaskEntity(request.TaskDto.Title, request.TaskDto.Description);
                task.Priority = request.TaskDto.Priority;
                task.DueDate = request.TaskDto.DueDate;
                task.CreatedAt = DateTime.UtcNow;
                task.UpdatedAt = DateTime.UtcNow;

                task = await _taskRepository.AddAsync(task, cancellationToken);

                if (activity != null)
                {
                    Tracing.AddEvent(activity, "TaskCreated", 
                        ("TaskId", task.Id),
                        ("State", task.State));
                }

                await _messageBus.PublishTaskCreatedEvent(task);

                return _mapper.Map<TaskDto>(task);
            }
            catch (Exception ex)
            {
                if (activity != null)
                {
                    Tracing.AddEvent(activity, "CreateTaskError", 
                        ("Error", ex.Message));
                }
                throw;
            }
        }
    }
} 