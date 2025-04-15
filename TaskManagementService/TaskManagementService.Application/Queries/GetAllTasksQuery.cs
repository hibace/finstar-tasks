using AutoMapper;
using MediatR;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.Application.Queries
{
    /// <summary>
    /// Запрос получения всех задач
    /// </summary>
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
    }

    /// <summary>
    /// Обработчик запроса получения всех задач
    /// </summary>
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GetAllTasksQueryHandler"/>
        /// </summary>
        /// <param name="taskRepository">Репозиторий задач</param>
        /// <param name="mapper">Объект для маппинга</param>
        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает запрос получения всех задач
        /// </summary>
        /// <param name="request">Запрос получения всех задач</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список всех задач</returns>
        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }
    }
} 