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

    ///// <summary>
    ///// Initializes an empty instance of the Milestone class.
    ///// </summary>
    //public Milestone()
    //    : this(0, "", "", TaskStatus.Unscheduled, null, null, null, null, 0, null, Enumerable.Empty<BO.TaskInList>().ToList()) { }

    ///// <summary>
    ///// Initializes an instance of the Milestone class with specified values.
    ///// </summary>
    //public Milestone(int id, string alias, string description, TaskStatus status, DateTime? createdAtDate, DateTime? forecastDate, DateTime? deadlineDate, DateTime? completeDate, double? completionPercentage, string? remarks, List<TaskInList> dependencies)
    //{
    //    Id = id;
    //    Alias = alias;
    //    Description = description;
    //    Status = status;
    //    CreatedAtDate = createdAtDate;
    //    ForecastDate = forecastDate;
    //    DeadlineDate = deadlineDate;
    //    CompleteDate = completeDate;
    //    CompletionPercentage = completionPercentage;
    //    Remarks = remarks;
    //    Dependencies = dependencies;
    //}

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
