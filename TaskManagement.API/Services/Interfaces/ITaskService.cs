using TaskManagement.API.Common;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Models.Enums;

namespace TaskManagement.API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto, Guid userId);
        Task<TaskResponseDto> GetByIdAsync(Guid id);
        Task<List<TaskResponseDto>> GetAllAsync(
            TaskStatusEnum? status,
            TaskPriorityEnum? priority,
            Guid? assignedToUserId);

        Task UpdateAsync(Guid id, UpdateTaskDto dto);
        Task UpdateStatusAsync(Guid id, UpdateStatusDto dto);
        Task DeleteAsync(Guid id, Guid userId, bool isAdmin);
        Task<PagedResponseDto<TaskResponseDto>> GetPagedAsync(PagedRequestDto dto);
    }
}
