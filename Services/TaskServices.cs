using TaskManagerApi.Models;
using TaskManagerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService

    {
        private readonly TaskDbContext _context;
        public TaskService(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        { return await _context.Tasks.ToListAsync(); }
        public async Task<TaskItem?> GetByIdAsync(int id)
        { return await _context.Tasks.FindAsync(id); }
        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, TaskItem updatedTask)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return false;

            task.Name = updatedTask.Name;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetCompletedAsync()
        {
            return await _context.Tasks
                .Where(t => t.IsCompleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetPendingAsync()
        {
            return await _context.Tasks
                .Where(t => !t.IsCompleted)
                .ToListAsync();
        }
        public async Task<bool> DeleteCompletedAsync()
        {
            var completedTask = await _context.Tasks
                .Where(t => t.IsCompleted)
                .ToListAsync();
            _context.Tasks.RemoveRange(completedTask);
            await _context.SaveChangesAsync();

            return true;

        }
        public async Task<bool> CompleteAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return false;
            task.IsCompleted = true;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> CountAsync()
        {
            var count = await _context.Tasks.CountAsync();
            return count > 0;
        }

        public async Task<TaskStats> GetStatsAsync()
        {
            var total = await _context.Tasks.CountAsync();
            var completed = await _context.Tasks.CountAsync(t => t.IsCompleted);
            var pending = total - completed;
            var percentageCompleted = total == 0 ? 0 : Math.Round((double)completed / total * 100, 2);


            var stats = new TaskStats()

            {
                Total = total,
                Completed = completed,
                Pending = pending,
                PercentageCompleted = percentageCompleted
            };

            return stats;
        }
        public async Task<TaskStatsDetailed> GetDetailedAsync()

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

            return detailed;

        }
        public async Task<IEnumerable<TaskItem>> SearchAsync(string title)
        { 
            var results = await _context.Tasks
                .Where(t => t.Name.Contains(title))
                .ToListAsync();
            return results;
        }    
        public async Task<IEnumerable<TaskItem>> GetByCategoryAsync(string name)
        {
            var results = await _context.Tasks
                .Where(t=>t.Category.Name.ToLower() == name.ToLower())
                .ToListAsync();
            return results;
        }
        public async Task<bool> PatchToogleTaskAsync(int id)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return false;
            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            return await _context.Tasks
                .Where(t => t.IsCompleted)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<TaskItem>> GetSortedAsync(string sort)
        {
            var query = _context.Tasks.AsQueryable();

            switch (sort)
            {
                case "name":
                    query = query.OrderBy(t => t.Name);
                    break;

                case "status":
                    query = query.OrderBy(t => t.IsCompleted);
                    break;

                case "newest":
                    query.OrderBy(t => t.Id);
                    break;

                default:
                    query = query.OrderBy(t => t.Id);
                    break;
            }

            return await query.ToListAsync();
        }
    }
}
