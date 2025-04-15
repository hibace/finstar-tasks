using MediatR;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Core.Telemetry;

namespace TaskManagementService.Application.Commands
{
    /// <summary>
    /// Команда удаления задачи
    /// </summary>
    public class DeleteTaskCommand : IRequest
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public Guid TaskId { get; }

        public DeleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }

    /// <summary>
    /// Обработчик команды удаления задачи
    /// </summary>
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageBus _messageBus;

        public DeleteTaskCommandHandler(
            ITaskRepository taskRepository,
            IMessageBus messageBus)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        }

        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            using var activity = Tracing.StartActivity("DeleteTask");
            try
            {
                if (activity != null)
                {
                    Tracing.AddTag(activity, "task.id", request.TaskId);
                }

                var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
                if (task == null)
                {
                    return;
                }

                await _taskRepository.DeleteAsync(task, cancellationToken);

                if (activity != null)
                {
                    Tracing.AddEvent(activity, "TaskDeleted", 
                        ("TaskId", task.Id));
                }

                await _messageBus.PublishTaskDeletedEvent(request.TaskId);
            }
            catch (Exception ex)
            {
                if (activity != null)
                {
                    Tracing.AddEvent(activity, "DeleteTaskError", 
                        ("Error", ex.Message));
                }
                throw;
            }
        }
    }
} 