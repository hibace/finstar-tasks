using AutoMapper;
using Moq;
using TaskManagementService.Application.Commands;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Domain.Exceptions;
using TaskManagementService.Shared.Dtos;
using Xunit;

namespace TaskManagementService.Tests.Commands
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMessageBus> _messageBus;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _messageBus = new Mock<IMessageBus>();
            _handler = new UpdateTaskCommandHandler(_mockRepository.Object, _mockMapper.Object, _messageBus.Object);
        }

        [Fact]
        public async Task Handle_WhenTaskExists_UpdatesTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new TaskEntity("Original Title", "Original Description")
            {
                Id = taskId,
                State = TaskState.Created,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var updateDto = new UpdateTaskDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                State = TaskState.InProgress,
                Priority = TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(2)
            };

            _mockRepository.Setup(r => r.GetByIdAsync(taskId, CancellationToken.None))
                .ReturnsAsync(existingTask);

            var command = new UpdateTaskCommand(taskId, updateDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Updated Title", existingTask.Title);
            Assert.Equal("Updated Description", existingTask.Description);
            Assert.Equal(TaskState.InProgress, existingTask.State);
            Assert.Equal(TaskPriority.High, existingTask.Priority);
            _mockRepository.Verify(r => r.UpdateAsync(existingTask, CancellationToken.None), Times.Once);
            _messageBus.Verify(m => m.PublishTaskUpdatedEvent(existingTask), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenTaskDoesNotExist_ThrowsException()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updateDto = new UpdateTaskDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                State = TaskState.InProgress,
                Priority = TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(2)
            };

            _mockRepository.Setup(r => r.GetByIdAsync(taskId, CancellationToken.None))
                .ReturnsAsync((TaskEntity?)null);

            var command = new UpdateTaskCommand(taskId, updateDto);

            // Act & Assert
            await Assert.ThrowsAsync<TaskNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }
    }
} 