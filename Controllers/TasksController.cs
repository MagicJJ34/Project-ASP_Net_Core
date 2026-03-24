using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerApi.Services;
using TaskManagerApi.DTOs;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAllTasks()
        {
            var data = await _taskService.GetAllTasksAsync();

            var response = _mapper.Map<IEnumerable<TaskResponseDto>>(data);
            
            return Ok(response);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
        {
            var data = await _taskService.GetTaskByIdAsync(id);

            var response = new TaskResponseDto
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                IsCompleted = data.IsCompleted,
            };

                if (data == null)
                    return NotFound(new { message = $"Zadanie o ID {id} nie istnieje" });

                return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)

        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
            };

            var created = await _taskService.CreateTaskAsync(task);

            var response = new TaskResponseDto
            {
                Id = created.Id,
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = created.IsCompleted,
            };

            return CreatedAtAction(nameof(GetTaskById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
            };
            var success = await _taskService.UpdateTaskAsync(id, task);

            if (!success)
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

        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> SearchTasks(string title)
        {
            var data= await _taskService.SearchTasksAsync(title);

            var response = data.Select(task => new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
            });

            return Ok(response);
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
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetPaged(int page = 1, int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page i PageSize musi być większe od 0");

            var data = await _taskService.GetPagedAsync(page, pageSize);

            var response = data.Select(task => new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
            });

            return Ok(response);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetSortedTasks([FromQuery]string sort)
        {
            var data = await _taskService.GetSortedTasksAsync(sort);

            var response = data.Select(task => new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
            });
            return Ok(response);
        }
        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksWithFilters(
            [FromQuery] string? search ,
            [FromQuery] string? sort,
            [FromQuery] string? category,
            [FromQuery] bool? status,
            int page = 1,
            int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page i PageSize musi być większe od 0");

            }
            var data = await _taskService.GetTasksWithFilters(search, sort, category, status, page, pageSize);

            var response = data.Select(task => new TaskResponseDto
            {
                Id= task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
            });

            return Ok(response);
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