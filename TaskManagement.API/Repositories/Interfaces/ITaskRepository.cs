using TaskManagement.API.Models.Enums;
using TaskManagement.API.Models;

namespace TaskManagement.API.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<List<TaskItem>> GetAllAsync(
            TaskStatusEnum? status,
            TaskPriorityEnum? priority,
            Guid? assignedToUserId);

        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);
        Task<(List<TaskItem> Tasks, int TotalCount)> 
            GetPagedAsync(int pageNumber, int pageSize, string sortOrder);
    }
}
