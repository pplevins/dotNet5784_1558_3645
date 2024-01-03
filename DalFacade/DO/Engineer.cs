namespace DO;

/// <summary>
/// Engineer Entity represents an engineer with all its properties
/// </summary>
/// <param name="Id">Unique identifier for the engineer</param>
/// <param name="Name">Name of the engineer</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineering Level assigned to the engineer</param>
/// <param name="Cost">Cost of the engineer per hour</param>
public record Engineer
(
    int Id,
    string Name,
    string Email,
    EngineerExperience Level,
    double? Cost = null
)
{
    /// <summary>
    /// Empty constructor for Engineer record.
    /// </summary>
    public Engineer() : this(0, "", "", EngineerExperience.Beginner, null) { }
}
