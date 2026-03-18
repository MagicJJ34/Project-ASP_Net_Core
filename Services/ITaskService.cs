using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<bool> UpdateAsync(int id, TaskItem updatekTask);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TaskItem>> GetCompletedAsync();
        Task<IEnumerable<TaskItem>> GetPendingAsync();
        Task<bool> DeleteCompletedAsync();
        Task<bool> CompleteAsync(int id);
        Task<bool> CountAsync();
        Task<TaskStats> GetStatsAsync();
        Task<TaskStatsDetailed> GetDetailedAsync();
        Task<IEnumerable<TaskItem>> SearchAsync(string title);
        Task<IEnumerable<TaskItem>> GetByCategoryAsync(string name);
        Task<bool> PatchToogleTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize);
        Task<IEnumerable<TaskItem>> GetSortedAsync(string sort);
        Task<IEnumerable<TaskItem>> GetSearchSortPageAsync(string? search, string? sort, string? category, bool? status, int page, int pageSize);
    }
}