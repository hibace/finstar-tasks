using Moq;
using TaskManagementService.Application.Commands;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Domain.Exceptions;
using Xunit;

namespace TaskManagementService.Tests.Commands
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly Mock<IMessageBus> _messageBus;
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _messageBus = new Mock<IMessageBus>();
            _handler = new DeleteTaskCommandHandler(_mockRepository.Object, _messageBus.Object);
        }

        [Fact]
        public async Task Handle_WhenTaskExists_DeletesTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskEntity("Test Task", "Test Description")
            {
                Id = taskId,
                State = TaskState.Created,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            _mockRepository.Setup(r => r.GetByIdAsync(It.Is<Guid>(id => id == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var command = new DeleteTaskCommand(taskId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.Is<TaskEntity>(t => t.Id == taskId), It.IsAny<CancellationToken>()), Times.Once);
            _messageBus.Verify(m => m.PublishTaskDeletedEvent(It.Is<Guid>(id => id == taskId)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenTaskDoesNotExist_ThrowsException()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(It.Is<Guid>(id => id == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskEntity?)null);

            var command = new DeleteTaskCommand(taskId);

            // Act & Assert
            await Assert.ThrowsAsync<TaskNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ShouldRethrow()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskEntity("Test Task", "Test Description")
            {
                Id = taskId,
                Priority = TaskPriority.Medium,
                State = TaskState.Created,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.GetByIdAsync(taskId, CancellationToken.None))
                .ReturnsAsync(task);

            _mockRepository.Setup(r => r.DeleteAsync(task, CancellationToken.None))
                .ThrowsAsync(new Exception("Test exception"));

            var command = new DeleteTaskCommand(taskId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => 
                _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(r => r.GetByIdAsync(taskId, CancellationToken.None), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(task, CancellationToken.None), Times.Once);
            _messageBus.Verify(m => m.PublishTaskDeletedEvent(It.IsAny<Guid>()), Times.Never);
        }
    }
} 