namespace DO;

/// <summary>
/// Dependency Entity represents a dependency between tasks in a project.
/// </summary>
/// <param name="Id">Unique identifier for the dependency</param>
/// <param name="PreviousTask">Identifier of the previous task</param>
/// <param name="DependentOnTask">Identifier of the task that is being depended upon</param>
public record Dependency
(
    int Id,
    int PreviousTask,
    int DependentOnTask
)
{
    /// <summary>
    /// Empty constructor for Dependency record.
    /// </summary>
    public Dependency()
           : this(0, 0, 0) { }
}
