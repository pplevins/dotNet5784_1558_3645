namespace BO;
/// <summary>
/// EngineerInTask Entity represents an Engineer with its core identifying properties 
/// </summary>
/// <param name="Id">Unique identifier for the Engineer (created automatically)</param>
/// <param name="Name">Name of the Engineer</param>
public class EngineerInTask
{
    public int Id { get; set; }
    public string Name { get; set; }


    /// <summary>
    /// Initializes an empty instance of the EngineerInTask class.
    /// </summary>
    public EngineerInTask()
        : this(0, "") { }

    /// <summary>
    /// Initializes an instance of the EngineerInTask class with specified values.
    /// </summary>
    public EngineerInTask(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
