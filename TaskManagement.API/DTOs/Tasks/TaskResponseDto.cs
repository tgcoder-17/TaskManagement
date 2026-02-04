using TaskManagement.API.Models.Enums;

namespace TaskManagement.API.DTOs.Tasks
{
    /// <summary>
    /// Response payload for task details
    /// </summary>
    public class TaskResponseDto
    {
        /// <summary>
        /// Unique identifier of the task
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Title of the task
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Detailed description of the task
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Status of the task (Pending, InProgress, Completed)
        /// </summary>
        public TaskStatusEnum Status { get; set; }
        /// <summary>
        /// Priority of the task (Low, Medium, High)
        /// </summary>
        public TaskPriorityEnum Priority { get; set; }
        /// <summary>
        /// Due date for the task
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// User ID to whom the task is assigned
        /// </summary>
        public Guid AssignedToUserId { get; set; }
        /// <summary>
        /// Timestamp when the task was created
        /// </summary>
        public Guid CreatedBy { get; set; }
    }
}
