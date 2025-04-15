using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementService.API.Controllers;
using TaskManagementService.Application.Commands;
using TaskManagementService.Application.Queries;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Shared.Dtos;
using Xunit;

namespace TaskManagementService.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TasksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<TaskDto>
            {
                new TaskDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Task 1",
                    Description = "Test Description 1",
                    State = TaskState.Created,
                    Priority = TaskPriority.Medium
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(okResult.Value);
            Assert.Equal(tasks, returnedTasks);
        }

        [Fact]
        public async Task GetById_WithExistingId_ReturnsOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDto
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description",
                State = TaskState.Created,
                Priority = TaskPriority.Medium
            };

            _mediatorMock.Setup(m => m.Send(It.Is<GetTaskByIdQuery>(q => q.TaskId == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskDto);

            // Act
            var result = await _controller.GetById(taskId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(taskDto, returnedTask);
        }

        [Fact]
        public async Task GetById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.Is<GetTaskByIdQuery>(q => q.TaskId == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskDto?)null);

            // Act
            var result = await _controller.GetById(taskId, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test Description",
                Priority = TaskPriority.Medium
            };

            var createdTaskDto = new TaskDto
            {
                Id = Guid.NewGuid(),
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                State = TaskState.Created,
                Priority = createTaskDto.Priority
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdTaskDto);

            // Act
            var result = await _controller.Create(createTaskDto, CancellationToken.None);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TasksController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(createdTaskDto.Id, createdAtActionResult.RouteValues?["id"]);
            var returnedTask = Assert.IsType<TaskDto>(createdAtActionResult.Value);
            Assert.Equal(createdTaskDto, returnedTask);
        }

        [Fact]
        public async Task Update_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updateTaskDto = new UpdateTaskDto
            {
                Title = "Updated Task",
                Description = "Updated Description",
                State = TaskState.InProgress,
                Priority = TaskPriority.High
            };

            var updatedTaskDto = new TaskDto
            {
                Id = taskId,
                Title = updateTaskDto.Title,
                Description = updateTaskDto.Description,
                State = updateTaskDto.State,
                Priority = updateTaskDto.Priority
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedTaskDto);

            // Act
            var result = await _controller.Update(taskId, updateTaskDto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(updatedTaskDto, returnedTask);
        }

        [Fact]
        public async Task Delete_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(taskId, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
} 