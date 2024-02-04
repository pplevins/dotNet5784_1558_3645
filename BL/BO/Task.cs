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
    public int Id { get; set; }
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
    /// Initializes an empty instance of the Task class.
    /// </summary>
    public Task()
        : this(0, "", "", "", EngineerExperience.Beginner, TaskStatus.Unscheduled, Enumerable.Empty<BO.TaskInList>().ToList(), null, TimeSpan.Zero, null, null, null, null, null, null, null, null) { }

    /// <summary>
    /// Initializes an instance of the Task class with specified values.
    /// </summary>
    public Task(
        int id,
        string alias,
        string description,
        string deliverables,
        EngineerExperience difficultyLevel,
        TaskStatus status,
        List<BO.TaskInList> dependencies,
        MilestoneInTask milestone,
        TimeSpan? requiredEffortTime = null,
        DateTime? createdAtDate = null,
        EngineerInTask? engineer = null,
        string? remarks = null,
        DateTime? scheduledDate = null,
        DateTime? startDate = null,
        DateTime? deadlineDate = null,
        DateTime? completeDate = null,
        DateTime? estimatedDate = null)
    {
        Id = id;
        Alias = alias;
        Description = description;
        Milestone = milestone;
        Deliverables = deliverables;
        DifficultyLevel = difficultyLevel;
        Status = status;
        Dependencies = dependencies;
        RequiredEffortTime = requiredEffortTime;
        CreatedAtDate = createdAtDate;
        Engineer = engineer;
        Remarks = remarks;
        ScheduledDate = scheduledDate;
        StartDate = startDate;
        DeadlineDate = deadlineDate;
        CompleteDate = completeDate;
        EstimatedDate = estimatedDate;
    }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
