using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Common;
using TaskManagement.API.DTOs.Tasks;
using TaskManagement.API.Services.Interfaces;

namespace TaskManagement.API.Controllers.V2
{
    /// <summary>
    /// Manages task-related operations (v2)
    /// </summary>
    [ApiController]
    [Route("api/v2/task")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves paginated tasks (v2)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(
            ApiResponse<PagedResponseDto<TaskResponseDto>>),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] PagedRequestDto request)
        {
            var result = await _taskService.GetPagedAsync(request);

            return Ok(ApiResponse<PagedResponseDto<TaskResponseDto>>
                .Success(result));
        }
    }
}
