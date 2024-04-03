namespace BO;

/// <summary>
/// Engineer Entity represents an engineer with all its properties
/// </summary>
/// /// <param name="Id">Unique identifier for the user</param>
///  <param name="Id">Unique identifier for the user</param>
/// <param name="Password">Password  of the user</param>
/// <param name="UserPermission">User Permission Level assigned to the user </param>
public class User
{
    public int Id { get; init; }
    public string Password { get; set; }
    public string Email { get; set; }
    public UserPermission UserPermission { get; set; }

    /// <summary>
    /// Overrides the ToString method to return the result of ToStringProperty.
    /// </summary>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
