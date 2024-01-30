namespace DO;

/// <summary>
/// Engineer Entity represents an engineer with all its properties
/// </summary>
/// /// <param name="Id">Unique identifier for the engineer (as in national id card)</param>
/// <param name="Name">Name of the engineer (full name)</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineering Level assigned to the engineer</param>
/// <param name="IsActive">The field to check if the entity is removed or not</param>
/// <param name="Cost">Cost of the engineer per hour</param>
/// <param name="Task">Identifier of the task assigned to the engineer</param>
public class Engineer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public EngineerExperience Level { get; set; }
    public bool IsActive { get; set; }
    public double? Cost { get; set; }
    public TaskInEngineer? Task { get; set; }

    /// <summary>
    /// Initializes an empty instance of the Engineer class.
    /// </summary>
    public Engineer()
        : this(0, "", "", EngineerExperience.Beginner, true, null, null) { }

    /// <summary>
    /// Initializes an instance of the Engineer class with specified values.
    /// </summary>
    public Engineer(int id, string name, string email, EngineerExperience level, bool isActive = true, double? cost = null, TaskInEngineer? task = null)
    {
        Id = id;
        Name = name;
        Email = email;
        Level = level;
        IsActive = isActive;
        Cost = cost;
        Task = task;
    }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
