namespace BO;
/// <summary>
/// TaskInEngineer Entity represents a Task with its core identifying properties
/// <param name="Id">Unique identifier for the task</param>
/// <param name="Alias">Alias of the task</param>
/// </summary>
public class TaskInEngineer
{
    public int Id { get; init; }
    public string? Alias { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
};
