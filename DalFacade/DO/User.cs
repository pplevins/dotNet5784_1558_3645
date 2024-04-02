namespace DO;

/// <summary>
/// Engineer User represents an user with all its properties
/// </summary>
/// <param name="Id">Unique identifier for the user</param>
/// <param name="Password">Password of the user</param>
/// <param name="Email">Email address of the user</param>
/// <param name="UserPermission">User Permission Level assigned to the user </param>
/// /// <param name="IsActive">The field to check if the entity is removed or not</param>
public record User
(
    int Id,
    string Password,
    UserPermission UserPermission,
    string Email,
    bool IsActive = true
)
{
    /// <summary>
    /// Initializes an empty instance of the User record.
    /// We opted not to create a parameterized constructor since the record type already has one.
    /// </summary>
    public User() : this(0, "", UserPermission.Engineer, "", true) { }
}