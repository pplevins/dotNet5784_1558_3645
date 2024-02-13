namespace BO;

/// <summary>
/// MilestoneInTask Entity represents a Milestone with its core identifying properties
/// </summary>
/// <param name="Id">Unique identifier for the Milestone (created automatically)</param>
/// <param name="Alias">Alias of the Milestone</param>
public class MilestoneInTask
{
    public int Id { get; init; }
    public string Alias { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
