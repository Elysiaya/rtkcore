using Rtk.Web.Services;

public class TaskViewModel
{
    public string TaskId { get; set; } = "";
    public string ResultPath { get; set; } = "";
    public bool IsCompleted { get; set; }
    public RtkTaskStatus Status { get; set; } = RtkTaskStatus.Queued;
    public string? Error { get; set; }
}