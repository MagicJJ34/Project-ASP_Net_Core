using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SQLitePCL;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskItem>> GetById(int id)

        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)

        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            task.Name = updatedTask.Name;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)

        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpGet("completed")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCompleted()

        {
            var completedTasks = await _context.Tasks
                .Where(t => t.IsCompleted)
                .ToListAsync();

            return Ok(completedTasks);
        }

        [HttpPatch("{id}/complete")]

        public async Task<IActionResult> Complete(int id)

        {
            var task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
            if (task == null) return NotFound();

            task.IsCompleted = true;
            return NoContent();

        }

        [HttpGet("pending")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetPending()

        {
            var pendingTasks = await _context.Tasks
                .Where(t => !t.IsCompleted)
                .ToListAsync();

            return Ok(pendingTasks);
        }

        [HttpGet("count")]

        public async Task<IActionResult> Count()

        {
            var count = await _context.Tasks.CountAsync();

            return Ok(count);
        }

        [HttpDelete("completed")]

        public async Task<IActionResult> Delete_Completed()

        {
            var completedTasks = _context.Tasks.Where(t => t.IsCompleted).ToList();
            _context.Tasks.RemoveRange(completedTasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("stats")]

        public async Task<ActionResult<TaskStats>> GetStats()

        {
            var total = await _context.Tasks.CountAsync();
            var completed = await _context.Tasks.CountAsync(t => t.IsCompleted);
            var pending = total - completed;
            var percentageCompleted = total == 0 ? 0 : Math.Round((double)completed / total * 100, 2);

            var stats = new TaskStats

            {
                Total = total,
                Completed = completed,
                Pending = pending,
                PercentageCompleted = percentageCompleted
            };

            return Ok(stats);
        }

        [HttpGet("detailed")]

        public async Task<ActionResult<TaskStatsDetailed>> GetDetailed()

        {
            var total = await _context.Tasks.CountAsync();
            var completed = await _context.Tasks.CountAsync(t => t.IsCompleted);
            var pending = total - completed;
            var PercentageCompleted = total == 0 ? 0 : Math.Round((double)completed / total * 100, 2);
            var PercentagePending = total == 0 ? 0 : Math.Round((double)pending / total * 100, 2);
            var Empty = total == 0 ? true : false;

            var detailed = new TaskStatsDetailed

            {
                Total = total,
                Completed = completed,
                Pending = pending,
                PercentageCompleted = PercentageCompleted,
                PercentagePending = PercentagePending,
                Empty = Empty
            };

            return Ok(detailed);
        }

        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<TaskItem>>> Search(string title)
        {
            var results = await _context.Tasks
                .Where(t => t.Name.Contains(title))
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("category/{name}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetByCategory (string name)
        {
            var tasks = await _context.Tasks
                .Where(t => t.Category.Name.ToLower() == name.ToLower())
                .ToListAsync();
            return Ok(tasks);
        }    
    }
}
