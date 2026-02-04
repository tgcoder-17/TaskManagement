using TaskManagement.API.Common.Exceptions;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Models.Enums;
using TaskManagement.API.Models;
using TaskManagement.API.Repositories.Interfaces;
using TaskManagement.API.Services.Interfaces;
using AutoMapper;

namespace TaskManagement.API.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;


        public TaskService(
            ITaskRepository taskRepo,
            IUserRepository userRepo,
            IMapper mapper)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto, Guid userId)
        {
            if (!await _userRepo.UserExistsAsync(dto.AssignedToUserId))
                throw new BadRequestException("Validation failed",
                    new List<string> { "Assigned user does not exist" });

            var task = _mapper.Map<TaskItem>(dto);
            task.Id = Guid.NewGuid();
            task.CreatedBy = userId;

            await _taskRepo.AddAsync(task);
            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<TaskResponseDto> GetByIdAsync(Guid id)
        {
            var task = await _taskRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Task not found");

            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<List<TaskResponseDto>> GetAllAsync(
            TaskStatusEnum? status,
            TaskPriorityEnum? priority,
            Guid? assignedToUserId)
        {
            var tasks = await _taskRepo.GetAllAsync(status, priority, assignedToUserId);
            return _mapper.Map<List<TaskResponseDto>>(tasks);
        }

        public async Task UpdateAsync(Guid id, UpdateTaskDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Task not found");

            _mapper.Map(dto, task);
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepo.UpdateAsync(task);
        }

        public async Task UpdateStatusAsync(Guid id, UpdateStatusDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Task not found");

            task.Status = dto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepo.UpdateAsync(task);
        }

        public async Task DeleteAsync(Guid id, Guid userId, bool isAdmin)
        {
            var task = await _taskRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Task not found");

            if (!isAdmin && task.CreatedBy != userId)
                throw new BadRequestException("You are not allowed to delete this task");

            await _taskRepo.DeleteAsync(task);
        }
    }
}
