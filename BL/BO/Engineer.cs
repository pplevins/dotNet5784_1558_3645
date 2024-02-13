namespace BO;

/// <summary>
/// Engineer Entity represents an engineer with all its properties
/// </summary>
/// /// <param name="Id">Unique identifier for the engineer (as in national id card)</param>
/// <param name="Name">Name of the engineer (full name)</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineering Level assigned to the engineer</param>
/// <param name="Cost">Cost of the engineer per hour</param>
/// <param name="Task">Identifier of the task assigned to the engineer</param>
public class Engineer
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double? Cost { get; set; }
    public TaskInEngineer? Task { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
