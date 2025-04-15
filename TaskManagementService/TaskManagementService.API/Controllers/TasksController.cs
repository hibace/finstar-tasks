using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagementService.Application.Commands;
using TaskManagementService.Application.Queries;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.API.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TasksController"/>
        /// </summary>
        /// <param name="mediator">Медиатор для обработки команд и запросов</param>
        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получает список всех задач
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список задач</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(CancellationToken cancellationToken)
        {
            var tasks = await _mediator.Send(new GetAllTasksQuery(), cancellationToken);
            return Ok(tasks);
        }

        /// <summary>
        /// Получает задачу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача</returns>
        /// <response code="200">Задача найдена</response>
        /// <response code="404">Задача не найдена</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery(id), cancellationToken);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        /// <summary>
        /// Создает новую задачу
        /// </summary>
        /// <param name="createTaskDto">Данные для создания задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданная задача</returns>
        /// <response code="201">Задача успешно создана</response>
        /// <response code="400">Некорректные данные</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDto>> Create(CreateTaskDto createTaskDto, CancellationToken cancellationToken)
        {
            var task = await _mediator.Send(new CreateTaskCommand(createTaskDto), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        /// <summary>
        /// Обновляет существующую задачу
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="updateTaskDto">Данные для обновления задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленная задача</returns>
        /// <response code="200">Задача успешно обновлена</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="404">Задача не найдена</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> Update(Guid id, UpdateTaskDto updateTaskDto, CancellationToken cancellationToken)
        {
            var task = await _mediator.Send(new UpdateTaskCommand(id, updateTaskDto), cancellationToken);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Результат операции</returns>
        /// <response code="204">Задача успешно удалена</response>
        /// <response code="404">Задача не найдена</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);
            return NoContent();
        }
    }
} 