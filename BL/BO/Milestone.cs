namespace BO;

/// <summary>
/// Milestone Entity represents a Milestone in a project.
/// </summary>
/// <param name="Id">Unique identifier for the Milestone</param>
/// <param name="Alias">Alias of the Milestone</param>
/// <param name="Description">Description of the Milestone</param>
/// <param name="Status">Status of the Milestone</param>
/// <param name="CreatedAtDate">The date the Milestone created at</param>
/// <param name="Engineer">Identifier of the engineer assigned to the Milestone</param>
/// <param name="Remarks">Additional remarks related to the Milestone</param>
/// <param name="CreatedAtDate">The Creation date of the Milestone</param>
/// <param name="ForecastDate">The Forecast date of the Milestone</param>
/// <param name="DeadlineDate">Planned deadline date of the Milestone</param>
/// <param name="CompleteDate">Actual Completion date of the Milestone</param>
/// <param name="CompletionPercentage">The completion percentage of the Milestone</param>
/// <param name="Remarks">The remarks regarding the Milestone</param>
/// <param name="Dependencies">The list of Task Dependencies the Milestone is dependent on</param>
public class Milestone
{
    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime? CreatedAtDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<TaskInList> Dependencies { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
