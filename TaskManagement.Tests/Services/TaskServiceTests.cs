using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using TaskManagement.API.Common.Exceptions;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Models;
using TaskManagement.API.Models.Enums;
using TaskManagement.API.Repositories.Interfaces;
using TaskManagement.API.Services.Implementations;
using Xunit;

namespace TaskManagement.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepo = new Mock<ITaskRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockMapper = new Mock<IMapper>();

            _taskService = new TaskService(
                _mockTaskRepo.Object,
                _mockUserRepo.Object,
                _memoryCache,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnTask_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createDto = new CreateTaskDto { Title = "Test Task", AssignedToUserId = userId };
            var taskItem = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task", AssignedToUserId = userId };
            var responseDto = new TaskResponseDto { Id = taskItem.Id, Title = "Test Task" };

            _mockUserRepo.Setup(x => x.UserExistsAsync(createDto.AssignedToUserId)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<TaskItem>(createDto)).Returns(taskItem);
            _mockTaskRepo.Setup(x => x.AddAsync(taskItem)).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<TaskResponseDto>(taskItem)).Returns(responseDto);

            // Act
            var result = await _taskService.CreateAsync(createDto, userId);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Task");
            _mockTaskRepo.Verify(x => x.AddAsync(taskItem), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequest_WhenAssignedUserDoesNotExist()
        {
            // Arrange
            var createDto = new CreateTaskDto { AssignedToUserId = Guid.NewGuid() };
            _mockUserRepo.Setup(x => x.UserExistsAsync(createDto.AssignedToUserId)).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _taskService.CreateAsync(createDto, Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Validation failed");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTaskFromRepo_WhenNotCached()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskItem = new TaskItem { Id = taskId, Title = "Repo Task" };
            var responseDto = new TaskResponseDto { Id = taskId, Title = "Repo Task" };

            _mockTaskRepo.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(taskItem);
            _mockMapper.Setup(x => x.Map<TaskResponseDto>(taskItem)).Returns(responseDto);

            // Act
            var result = await _taskService.GetByIdAsync(taskId);

            // Assert
            result.Title.Should().Be("Repo Task");
            _mockTaskRepo.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockTaskRepo.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync((TaskItem)null);

            // Act
            Func<Task> act = async () => await _taskService.GetByIdAsync(taskId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Task not found");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTask_WhenUserIsAdmin()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var taskItem = new TaskItem { Id = taskId, CreatedBy = Guid.NewGuid() }; // Created by someone else

            _mockTaskRepo.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(taskItem);

            // Act
            await _taskService.DeleteAsync(taskId, userId, isAdmin: true);

            // Assert
            _mockTaskRepo.Verify(x => x.DeleteAsync(taskItem), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTask_WhenUserIsCreator()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var taskItem = new TaskItem { Id = taskId, CreatedBy = userId }; // Created by same user

            _mockTaskRepo.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(taskItem);

            // Act
            await _taskService.DeleteAsync(taskId, userId, isAdmin: false);

            // Assert
            _mockTaskRepo.Verify(x => x.DeleteAsync(taskItem), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowBadRequest_WhenUserIsNotAdminAndNotCreator()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var taskItem = new TaskItem { Id = taskId, CreatedBy = Guid.NewGuid() }; // Created by someone else

            _mockTaskRepo.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(taskItem);

            // Act
            Func<Task> act = async () => await _taskService.DeleteAsync(taskId, userId, isAdmin: false);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You are not allowed to delete this task");
        }
    }
}
