using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Common.Validation;
using TaskManagement.API.Models.Enums;

namespace TaskManagement.API.DTOs.Tasks
{
    /// <summary>
    /// Request payload for updating an existing task
    /// </summary>
    public class UpdateTaskDto
    {
        /// <summary>
        /// Title of the task
        /// </summary>
        [Required, StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Optional detailed description of the task
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Priority of the task (Low, Medium, High)
        /// </summary>
        [Required]
        [ValidEnum(typeof(TaskPriorityEnum))]
        public TaskPriorityEnum Priority { get; set; }

        /// <summary>
        /// Optional due date for the task
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// User ID to whom the task is assigned
        /// </summary>
        [Required]
        public Guid AssignedToUserId { get; set; }
    }
}
