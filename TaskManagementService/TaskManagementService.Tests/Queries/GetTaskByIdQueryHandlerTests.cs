using AutoMapper;
using Moq;
using TaskManagementService.Application.Queries;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Shared.Dtos;
using Xunit;

namespace TaskManagementService.Tests.Queries
{
    public class GetTaskByIdQueryHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetTaskByIdQueryHandler _handler;

        public GetTaskByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetTaskByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenTaskExists_ReturnsTaskDto()
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
            var taskDto = new TaskDto
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description",
                State = TaskState.Created,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            _mockRepository.Setup(r => r.GetByIdAsync(It.Is<Guid>(id => id == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);
            _mockMapper.Setup(m => m.Map<TaskDto>(It.Is<TaskEntity>(t => t.Id == taskId)))
                .Returns(taskDto);

            var query = new GetTaskByIdQuery(taskId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Test Task", result.Title);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal(TaskState.Created, result.State);
            Assert.Equal(TaskPriority.Medium, result.Priority);
        }

        [Fact]
        public async Task Handle_WhenTaskDoesNotExist_ReturnsNull()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(It.Is<Guid>(id => id == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskEntity?)null);

            var query = new GetTaskByIdQuery(taskId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ShouldRethrow()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdAsync(taskId, CancellationToken.None))
                .ThrowsAsync(new Exception("Test exception"));

            var query = new GetTaskByIdQuery(taskId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => 
                _handler.Handle(query, CancellationToken.None));

            _mockRepository.Verify(r => r.GetByIdAsync(taskId, CancellationToken.None), Times.Once);
            _mockMapper.Verify(m => m.Map<TaskDto>(It.IsAny<TaskEntity>()), Times.Never);
        }
    }
} 