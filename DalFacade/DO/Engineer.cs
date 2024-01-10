namespace DO;

/// <summary>
/// Engineer Entity represents an engineer with all its properties
/// </summary>
/// <param name="Id">Unique identifier for the engineer (as in national id card)</param>
/// <param name="Name">Name of the engineer (full name)</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineering Level assigned to the engineer</param>
/// /// <param name="IsActive">The field to check if the entity is removed or not</param>
/// <param name="Cost">Cost of the engineer per hour</param>
public record Engineer
(
    int Id,
    string Name,
    string Email,
    EngineerExperience Level,
    bool IsActive = true,
    double? Cost = null
)
{
    /// <summary>
    /// Initializes an empty instance of the Engineer record for stage 3, as instructed.
    /// We opted not to create a parameterized constructor since the record type already has one.
    /// </summary>
    public Engineer() : this(0, "", "", EngineerExperience.Beginner, true, null) { }
}
