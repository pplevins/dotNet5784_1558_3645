namespace DO;

/// <summary>
/// Dependency Entity represents a dependency between tasks in a project.
/// </summary>
/// <param name="Id">Unique identifier for the dependency</param>
/// <param name="DependentTask">The task ID that depends on other task.</param>
/// <param name="PreviousTask">The task ID that this task depends on (the previous task)</param>
/// <param name="IsActive">The field to check if the entity is removed or not</param>
public record Dependency
(
    int Id,
    int DependentTask,
    int PreviousTask,
    bool IsActive = true
)
{
    /// <summary>
    /// Initializes an empty instance of the Dependency record for stage 3, as instructed.
    /// We opted not to create a parameterized constructor since the record type already has one.
    /// </summary>
    public Dependency()
           : this(0, 0, 0, true) { }
}
