namespace BO;

/// <summary>
/// TaskInList Entity represents a task with its core identifying properties
/// </summary>
/// <param name="Id">Unique identifier for the task</param>
/// <param name="Alias">Alias of the task</param>
/// <param name="Description">Description of the task</param>
/// <param name="Status">Deliverables of the task</param>
public class TaskInList
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}