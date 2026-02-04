namespace TaskManagement.API.Models.Enums;

/// <summary>
/// Represents the current status of a task
/// </summary>
public enum TaskStatusEnum
{
    /// <summary>
    /// Task is created but not yet started
    /// </summary>
    Pending = 1,
    /// <summary>
    /// Task is currently in progress
    /// </summary>
    InProgress = 2,
    /// <summary>
    /// Task has been completed
    /// </summary>
    Completed = 3
}
