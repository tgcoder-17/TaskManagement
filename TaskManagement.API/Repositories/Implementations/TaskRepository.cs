using TaskManagement.API.Data;
using TaskManagement.API.Models.Enums;
using TaskManagement.API.Models;
using TaskManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TaskItem>> GetAllAsync(
            TaskStatusEnum? status,
            TaskPriorityEnum? priority,
            Guid? assignedToUserId)
        {
            var query = _db.Tasks.AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority);

            if (assignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == assignedToUserId);

            return await query.ToListAsync();
        }

        public async Task AddAsync(TaskItem task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }

        public async Task<(List<TaskItem> Tasks, int TotalCount)> 
            GetPagedAsync(int pageNumber, int pageSize, string sortOrder)
        {
            var query = _db.Tasks.AsNoTracking().AsQueryable();

            query = sortOrder == "asc"
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt);

            var totalCount = await query.CountAsync();

            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks, totalCount);
        }
    }
}
