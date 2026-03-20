using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            var data = await _taskService.GetAllTasksAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskItem>> GetTaskById(int id)
        {
                var data = await _taskService.GetTaskByIdAsync(id);

                if (data == null)
                    return NotFound(new { message = $"Zadanie o ID {id} nie istnieje" });

                return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)

        {
            var data = await _taskService.CreateTaskAsync(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = data.Id }, data);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTask(int id, TaskItem updatedTask)
        {
            var data = await _taskService.UpdateTaskAsync(id, updatedTask);

            if (!data)
                return NotFound(new { message = $"Zadanie o ID{id} nie istnieje"});

            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTask(int id)

        {
            var data = await _taskService.DeleteTaskAsync(id);

            if (!data)
                return NotFound(new {message = $"Zadanie o ID{id} nie istnieje"});
            return NoContent();
        }

        [HttpGet("completed")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCompletedTasks()

        {
            var data
                = await _taskService.GetCompletedTasksAsync();
            return Ok(data);
        }

        [HttpPatch("{id}/complete")]

        public async Task<IActionResult> PatchCompletedTasks(int id)

        {
            var data = await _taskService.PatchCompletedTasksAsync(id);
            if (!data)
                return NotFound(new {message=$"Zadanie o ID{id} nie istnieje"});

            return NoContent();

        }

        [HttpGet("pending")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetPendingTasks()

        {
            var data = await _taskService.GetPendingTasksAsync();

            return Ok(data);
        }

        [HttpGet("count")]

        public async Task<ActionResult<int>> CountTasks()

        {
            var data = await _taskService.CountTasksAsync();
            return Ok(data);
        }

        [HttpDelete("completed")]

        public async Task<IActionResult> DeleteCompletedTasks()

        {
            var data = await _taskService.DeleteCompletedTasksAsync();
            if (!data)
                return NotFound(new {message = $"Zadania nie istnieją"});
            return NoContent();
        }

        [HttpGet("stats")]

        public async Task<ActionResult<TaskStats>> GetTasksStats()

        {
            var data = await _taskService.GetTasksStatsAsync();

            return Ok(data);
        }

        [HttpGet("detailed")]

        public async Task<ActionResult<TaskStatsDetailed>> GetDetailedTasksStats()

        {
            var data = await _taskService.GetDetailedStatsTasksAsync();
            return Ok(data);
        }

        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> SearchTasks(string title)
        {
            var data= await _taskService.SearchTasksAsync(title);
            return Ok(data);
        }

        [HttpGet("category/{name}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetByCategory(string name)
        {
            var data = await _taskService.GetByCategoryAsync(name);
            return Ok(data);
        }

        [HttpPatch("toggle/{id}")]
        public async Task<IActionResult> ToggleTask(int id)
        {
            var data = await _taskService.PatchToggleTaskAsync(id);
            if (!data)
                return NotFound(new { message = $"Zadanie o ID{id} nie istnieje" } );
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetPaged(int page = 1, int pageSize = 5)
        {
            var data = await _taskService.GetPagedAsync(page, pageSize);
            return Ok(data);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetSortedTasks(string sort)
        {
            var data = await _taskService.GetSortedTasksAsync(sort);
            return Ok(data);
        }
        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksWithFilters(
            string? search ,
            string? sort,
            string? category,
            bool? status,
            int page = 1,
            int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page i PageSize musi być większe od 0");

            }
            var data = await _taskService.GetTasksWithFilters(search, sort, category, status, page, pageSize);
            return Ok(data);
        }

        [HttpGet("count-pending")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCountPendingTasksAsync()
        
        {
            var data = await _taskService.GetCountPendingTasksAsync();
            return Ok(data);
        }

        [HttpGet("simple-stats")]
        public async Task<ActionResult<TaskStats>> GetSimpleStatsAsync()
        {
            var data =  await _taskService.GetSimpleStatsAsync();
            return Ok(data);
        }
    }
}