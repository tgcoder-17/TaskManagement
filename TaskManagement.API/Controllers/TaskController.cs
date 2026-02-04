using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Common;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Models.Enums;
using TaskManagement.API.Services.Interfaces;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// Manages task-related operations
    /// </summary>
    [ApiController]
    [Route("api/v1/task")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Creates a new task
        /// </summary>
        /// <param name="dto">Task creation payload</param>
        /// <returns>The created task</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TaskResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var result = await _taskService.CreateAsync(dto, userId);

            return Ok(ApiResponse<TaskResponseDto>.Success(result));
        }

        /// <summary>
        /// Retrieves a task by its unique identifier
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TaskResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);

            return Ok(ApiResponse<TaskResponseDto>.Success(task));
        }

        /// <summary>
        /// Retrieves all tasks with optional filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TaskResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(
            [FromQuery] TaskStatusEnum? status,
            [FromQuery] TaskPriorityEnum? priority,
            [FromQuery] Guid? assignedTo)
        {
            var tasks = await _taskService.GetAllAsync(status, priority, assignedTo);
            return Ok(ApiResponse<List<TaskResponseDto>>.Success(tasks));
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
        {
            await _taskService.UpdateAsync(id, dto);

            return Ok(ApiResponse<object>.NoContent(
                message: "Task updated successfully"));
        }

        /// <summary>
        /// Updates only the status of a task
        /// </summary>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto dto)
        {
            await _taskService.UpdateStatusAsync(id, dto);
            return Ok(ApiResponse<object>.NoContent(
                message: "Task status updated successfully"));
        }

        /// <summary>
        /// Deletes a task (Admin or task owner only)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole(UserRoleEnum.Admin.ToString());

            await _taskService.DeleteAsync(id, userId, isAdmin);

            return Ok(ApiResponse<object>.NoContent(
                message: "Task deleted successfully"));
        }
    }
}
