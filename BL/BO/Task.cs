namespace BO;
/// <summary>
/// Task Entity represents a task with all its properties
/// </summary>
/// <param name="Id">Unique identifier for the task</param>
/// <param name="Alias">Alias of the task</param>
/// <param name="Description">Description of the task</param>
/// <param name="Deliverables">Deliverables of the task</param>
/// <param name="DifficultyLevel">Difficulty level of the task</param>
/// <param name="Status">Status of the task</param>
/// <param name="Dependencies">List of all Dependent tasks</param>
/// <param name="Milestone">Milestone of the Task</param>
/// <param name="RequiredEffortTime">Required numbers of days to complete the task</param>
/// <param name="CreatedAtDate">The date the task created at</param>
/// <param name="Engineer">Identifier of the engineer assigned to the task</param>
/// <param name="Remarks">Additional remarks related to the task</param>
/// <param name="ScheduledDate">Scheduled start date of the task</param>
/// <param name="StartDate">Actual start date of the task</param>
/// <param name="DeadlineDate">Planned deadline date of the task</param>
/// <param name="CompleteDate">Actual Completion date of the task</param>
/// <param name="EstimatedDate">Estimated date for the task completion.</param>
public class Task
{
    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public string Deliverables { get; set; }
    public EngineerExperience DifficultyLevel { get; set; }
    public TaskStatus Status { get; set; }
    public List<BO.TaskInList> Dependencies { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? CreatedAtDate { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public string? Remarks { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public DateTime? EstimatedDate { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
