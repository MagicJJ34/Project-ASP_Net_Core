using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(100, ErrorMessage = "Tytuł max 100 znaków")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Opis max 500 znaków")]
        public string Description { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(100, ErrorMessage = "Tytuł max 100 znaków")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Tytuł max 500 znaków")]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TaskResponseDto
    { 
        public int Id  { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
