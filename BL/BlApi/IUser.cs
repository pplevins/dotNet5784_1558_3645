using BO;

namespace BlApi;
public interface IUser
{
    /// <summary>
    /// Creates a new entity object in DAL object in DAL
    /// </summary>
    /// <param name="user">The new entity to create</param>
    /// <returns>The entity's ID</returns>
    int Create(User user);

    /// <summary>
    /// Reads an entity object by its ID
    /// </summary>
    /// <param name="id">The entity's ID</param>
    /// <returns>The entity</returns>
    User? Read(int id);
    User? Login(User user);
    void Delete(int id);
    /// <summary>
    /// Reset the entire DB
    /// Reset the entire DB
    /// </summary>
    /// <param name="id">The entity's ID</param>
    void Reset();
}

