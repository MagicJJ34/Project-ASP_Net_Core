using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<bool> UpdateTaskAsync(int id, TaskItem updatekTask);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetCompletedTasksAsync();
        Task<IEnumerable<TaskItem>> GetPendingTasksAsync();
        Task<bool> DeleteCompletedTasksAsync();
        Task<bool> PatchCompletedTasksAsync(int id);
        Task<bool> CountTasksAsync();
        Task<TaskStats> GetTasksStatsAsync();
        Task<TaskStatsDetailed> GetDetailedStatsTasksAsync();
        Task<IEnumerable<TaskItem>> SearchTasksAsync(string title);
        Task<IEnumerable<TaskItem>> GetByCategoryAsync(string name);
        Task<bool> PatchToggleTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize);
        Task<IEnumerable<TaskItem>> GetSortedTasksAsync(string sort);
        Task<IEnumerable<TaskItem>> GetTasksWithFilters(string? search, string? sort, string? category, bool? status, int page, int pageSize);
        Task<int> GetCountPendingTasksAsync();
        Task<TaskStats> GetSimpleStatsAsync();
    }
}