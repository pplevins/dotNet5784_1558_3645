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
    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime? CreatedAtDate { get; set; }
    public double? CompletionPercentage { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
