using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TaskManagement.API.Common;
using TaskManagement.API.Controllers;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Services.Interfaces;
using Xunit;

namespace TaskManagement.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _controller = new TaskController(_mockTaskService.Object);

            // Setup User context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenTaskIsCreated()
        {
            // Arrange
            var createDto = new CreateTaskDto { Title = "New Task" };
            var responseDto = new TaskResponseDto { Id = Guid.NewGuid(), Title = "New Task" };

            _mockTaskService.Setup(x => x.CreateAsync(createDto, It.IsAny<Guid>()))
                .ReturnsAsync(responseDto);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<TaskResponseDto>>().Subject;
            apiResponse.Data.Title.Should().Be("New Task");
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var responseDto = new TaskResponseDto { Id = taskId, Title = "Existing Task" };

            _mockTaskService.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(responseDto);

            // Act
            var result = await _controller.GetById(taskId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<TaskResponseDto>>().Subject;
            apiResponse.Data.Id.Should().Be(taskId);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<TaskResponseDto> { new TaskResponseDto(), new TaskResponseDto() };

            _mockTaskService.Setup(x => x.GetAllAsync(null, null, null))
                .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAll(null, null, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<List<TaskResponseDto>>>().Subject;
            apiResponse.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updateDto = new UpdateTaskDto { Title = "Updated Title" };

            _mockTaskService.Setup(x => x.UpdateAsync(taskId, updateDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(taskId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<object>>().Subject;
            apiResponse.Message.Should().Be("Task updated successfully");
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenDeleteIsSuccessful()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockTaskService.Setup(x => x.DeleteAsync(taskId, It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<object>>().Subject;
            apiResponse.Message.Should().Be("Task deleted successfully");
        }
    }
}
