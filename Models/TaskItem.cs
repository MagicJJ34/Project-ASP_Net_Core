using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace TaskManagerApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Zadanie musi mieć tytuł !")]
        [StringLength(100, ErrorMessage = "Tytuł nie może być dłuższy niż 100 znaków")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
