using AutoMapper;
using MediatR;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.Application.Queries
{
    /// <summary>
    /// Запрос получения задачи по ID
    /// </summary>
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public Guid TaskId { get; }

        public GetTaskByIdQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }

    /// <summary>
    /// Обработчик запроса получения задачи по ID
    /// </summary>
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            return _mapper.Map<TaskDto>(task);
        }
    }
} 