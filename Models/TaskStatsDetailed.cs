namespace TaskManagerApi.Models
{
    public class TaskStatsDetailed
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public double PercentageCompleted { get; set; }
        public double PercentagePending { get; set; }
        public bool Empty { get; set; }

    }
}
