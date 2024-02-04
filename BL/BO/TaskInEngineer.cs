namespace BO;
/// <summary>
/// TaskInEngineer Entity represents a Task with its core identifying properties
/// <param name="Id">Unique identifier for the task</param>
/// <param name="Alias">Alias of the task</param>
/// </summary>
public class TaskInEngineer
{
    public int Id { get; set; }
    public string Alias { get; set; }


    /// <summary>
    /// Initializes an empty instance of the TaskInEngineer class.
    /// </summary>
    public TaskInEngineer()
        : this(0, "") { }

    /// <summary>
    /// Initializes an instance of the TaskInEngineer class with specified values.
    /// </summary>
    public TaskInEngineer(int id, string alias)
    {
        Id = id;
        Alias = alias;
    }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
};
