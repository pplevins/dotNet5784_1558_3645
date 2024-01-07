namespace DO;

/// <summary>
/// Dependency Entity represents a dependency between tasks in a project.
/// </summary>
/// <param name="Id">Unique identifier for the dependency</param>
/// <param name="DependentTask">The task ID that depends on other task.</param>
/// <param name="PreviousTask">The task ID that this task depends on (the previous task)</param>
public record Dependency
(
    int Id,
    int DependentTask,
    int PreviousTask
)
{
    /// <summary>
    /// Empty constructor for Dependency record.
    /// </summary>
    public Dependency()
           : this(0, 0, 0) { }
}
