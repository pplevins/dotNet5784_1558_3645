namespace DO;

/// <summary>
/// MilestoneInTask Entity represents a Milestone with its core identifying properties
/// </summary>
/// <param name="Id">Unique identifier for the Milestone (created automatically)</param>
/// <param name="Alias">Alias of the Milestone</param>
public class MilestoneInTask
{
    public int Id { get; set; }
    public string Alias { get; set; }


    /// <summary>
    /// Initializes an empty instance of the MilestoneInTask class.
    /// </summary>
    public MilestoneInTask()
        : this(0, "") { }

    /// <summary>
    /// Initializes an instance of the MilestoneInTask class with specified values.
    /// </summary>
    public MilestoneInTask(int id, string alias)
    {
        Id = id;
        Alias = alias;
    }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
