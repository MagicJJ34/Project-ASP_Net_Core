using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SQLitePCL;
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
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskItem>> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);

                if (task == null)
                    return NotFound(new { message = $"Zadanie o ID {id} nie istnieje" });

                return Ok(task);
            }
            catch (Exception ex)

            {
                return StatusCode(500, "Wystąpił nieoczekiwany błąd serwera");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)

        {
            var createdTask = await _taskService.CreateTaskAsync(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTask(int id, TaskItem updatedTask)
        {
            var success = await _taskService.UpdateTaskAsync(id, updatedTask);

            if (!success)
                return NotFound();

            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTask(int id)

        {
            var success = await _taskService.DeleteTaskAsync(id);

            if (!success)
                return NotFound();
            return NoContent();

        }

        [HttpGet("completed")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCompletedTasks()

        {
            var completedTasks = await _taskService.GetCompletedTasksAsync();
            return Ok(completedTasks);
        }

        [HttpPatch("{id}/complete")]

        public async Task<IActionResult> PatchCompletedTasks(int id)

        {
            var success = await _taskService.PatchCompletedTasksAsync(id);
            if (!success)
                return NotFound();

            return NoContent();

        }

        [HttpGet("pending")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetPendingTasks()

        {
            var pendingTasks = await _taskService.GetPendingTasksAsync();

            return Ok(pendingTasks);
        }

        [HttpGet("count")]

        public async Task<IActionResult> CountTasks()

        {
            var count = await _taskService.CountTasksAsync();
            return Ok(count);
        }

        [HttpDelete("completed")]

        public async Task<IActionResult> DeleteCompletedTasks()

        {
            var success = await _taskService.DeleteCompletedTasksAsync();
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpGet("stats")]

        public async Task<ActionResult<TaskStats>> GetTasksStats()

        {
            var stats = await _taskService.GetTasksStatsAsync();

            return Ok(stats);
        }

        [HttpGet("detailed")]

        public async Task<ActionResult<TaskStatsDetailed>> GetDetailedTasksStats()

        {
            var detailed = await _taskService.GetDetailedStatsTasksAsync();
            return Ok(detailed);
        }

        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> SearchTasks(string title)
        {
            var results = await _taskService.SearchTasksAsync(title);
            return Ok(results);
        }

        [HttpGet("category/{name}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetByCategory(string name)
        {
            var tasks = await _taskService.GetByCategoryAsync(name);
            return Ok(tasks);
        }

        [HttpPatch("toogle/{id}")]
        public async Task<IActionResult> ToogleTask(int id)
        {
            var success = await _taskService.PatchToogleTaskAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetPaged(int page = 1, int pageSize = 5)
        {
            var tasks = await _taskService.GetPagedAsync(page, pageSize);
            return Ok(tasks);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetSortedTasks(string sort)
        {
            var tasks = await _taskService.GetSortedTasksAsync(sort);
            return Ok(tasks);
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
            var tasks = await _taskService.GetTasksWithFilters(search, sort, category, status, page, pageSize);
            return Ok(tasks);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCountPendingTasksAsync()
        
        {
            var tasks = await _taskService.GetCountPendingTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("simple-stats")]
        public async Task<TaskStats> GetSimpleStatsAsync()
        {
            return await _taskService.GetSimpleStatsAsync();
        }
    }
}