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
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);

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
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)

        {
            var createdTask = await _taskService.CreateAsync(task);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var success = await _taskService.UpdateAsync(id, updatedTask);

            if (!success)
                return NotFound();

            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)

        {
            var success = await _taskService.DeleteAsync(id);

            if (!success)
                return NotFound();
            return NoContent();

        }

        [HttpGet("completed")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCompleted()

        {
            var completedTasks = await _taskService.GetCompletedAsync();
            return Ok(completedTasks);
        }

        [HttpPatch("{id}/complete")]

        public async Task<IActionResult> Complete(int id)

        {
            var success = await _taskService.CompleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();

        }

        [HttpGet("pending")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetPending()

        {
            var pendingTasks = await _taskService.GetPendingAsync();

            return Ok(pendingTasks);
        }

        [HttpGet("count")]

        public async Task<IActionResult> Count()

        {
            var count = await _taskService.CountAsync();
            return Ok(count);
        }

        [HttpDelete("completed")]

        public async Task<IActionResult> Delete_Completed()

        {
            var success = await _taskService.DeleteCompletedAsync();
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpGet("stats")]

        public async Task<ActionResult<TaskStats>> GetStats()

        {
            var stats = await _taskService.GetStatsAsync();

            return Ok(stats);
        }

        [HttpGet("detailed")]

        public async Task<ActionResult<TaskStatsDetailed>> GetDetailed()

        {
            var detailed = await _taskService.GetDetailedAsync();
            return Ok(detailed);
        }

        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> Search(string title)
        {
            var results = await _taskService.SearchAsync(title);
            return Ok(results);
        }

        [HttpGet("category/{name}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetByCategory(string name)
        {
            var tasks = await _taskService.GetByCategoryAsync(name);
            return Ok(tasks);
        }

        [HttpPatch("toogle/{id}")]
        public async Task<IActionResult> Toogle(int id)
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
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetSorted(string sort)
        {
            var tasks = await _taskService.GetSortedAsync(sort);
            return Ok(tasks);
        }
    }
}
