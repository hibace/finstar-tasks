using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskManagementService.Application.Commands;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Shared.Dtos;
using Xunit;

namespace TaskManagementService.Tests.Commands
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMessageBus> _mockMessageBus;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMessageBus = new Mock<IMessageBus>();
            _handler = new CreateTaskCommandHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockMessageBus.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsCreatedTaskDto()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test Description",
                Priority = TaskPriority.Medium
            };

            var taskEntity = new TaskEntity(createTaskDto.Title, createTaskDto.Description)
            {
                Id = Guid.NewGuid(),
                State = TaskState.Created,
                Priority = createTaskDto.Priority,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var taskDto = new TaskDto
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                State = taskEntity.State,
                Priority = taskEntity.Priority,
                CreatedAt = taskEntity.CreatedAt,
                UpdatedAt = taskEntity.UpdatedAt
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskEntity);

            _mockMapper.Setup(m => m.Map<TaskDto>(taskEntity))
                .Returns(taskDto);

            var command = new CreateTaskCommand(createTaskDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskEntity.Id, result.Id);
            Assert.Equal(createTaskDto.Title, result.Title);
            Assert.Equal(createTaskDto.Description, result.Description);
            Assert.Equal(TaskState.Created, result.State);
            Assert.Equal(createTaskDto.Priority, result.Priority);

            _mockRepository.Verify(r => r.AddAsync(It.IsAny<TaskEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<TaskDto>(taskEntity), Times.Once);
            _mockMessageBus.Verify(m => m.PublishTaskCreatedEvent(It.IsAny<TaskEntity>()), Times.Once);
        }
    }
} 