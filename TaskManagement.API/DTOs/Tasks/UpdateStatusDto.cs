using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Common.Validation;
using TaskManagement.API.Models.Enums;

namespace TaskManagement.API.DTOs.Tasks
{
    /// <summary>
    /// Request payload for updating the status of a task
    /// </summary>
    public class UpdateStatusDto
    {
        /// <summary>
        /// New status of the task (Pending, InProgress, Completed)
        /// </summary>
        [Required]
        [ValidEnum(typeof(TaskStatusEnum))]
        public TaskStatusEnum Status { get; set; }
    }
}
