using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Models.Enums;

namespace TaskManagement.API.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;

        [Required]
        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;

        public DateTime? DueDate { get; set; }

        [Required]
        public Guid AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; } = null!;

        [Required]
        public Guid CreatedBy { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
