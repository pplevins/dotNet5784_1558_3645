namespace BO;
/// <summary>
/// EngineerInTask Entity represents an Engineer with its core identifying properties 
/// </summary>
/// <param name="Id">Unique identifier for the Engineer (created automatically)</param>
/// <param name="Name">Name of the Engineer</param>
public class EngineerInTask
{
    public int Id { get; init; }
    public string? Name { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
