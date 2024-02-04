namespace BO;

/// <summary>
/// MilestoneInList Entity represents a task with its core identifying properties
/// </summary>
/// <param name="Id">Unique identifier for the Milestone</param>
/// <param name="Alias">Alias of the Milestone</param>
/// <param name="Description">Description of the Milestone</param>
/// <param name="Status">status of the Milestone</param>
/// <param name="CreatedAtDate">The date the Milestone created at</param>
/// <param name="CompletionPercentage">The completion percentage of the Milestone</param>
public class MilestoneInList
{
    public int Id { get; set; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime? CreatedAtDate { get; set; }
    public double? CompletionPercentage { get; set; }

    /// <summary>
    /// Initializes an empty instance of the MilestoneInList class.
    /// </summary>
    public MilestoneInList() 
        : this(0, "", "", TaskStatus.Unscheduled, null, null) { }

    /// <summary>
    /// Initializes an instance of the MilestoneInList class with specified values.
    /// </summary>
    public MilestoneInList(int id, string alias, string description, TaskStatus status, DateTime? createdAtDate, double? completionPercentage)
    {
        Id = id;
        Alias = alias;
        Description = description;
        Status = status;
        CreatedAtDate = createdAtDate;
        CompletionPercentage = completionPercentage;
    }
}
