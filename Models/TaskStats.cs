namespace TaskManagerApi.Models
{
    public class TaskStats
    {
        public int Total {  get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public double PercentageCompleted { get; set; }
    }
}
