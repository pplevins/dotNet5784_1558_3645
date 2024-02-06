namespace BlApi;

public interface ICrud<T, U> where T : class where U : class
{
    /// <summary>
    /// Creates a new entity object in DAL object in DAL
    /// </summary>
    /// <param name="item">The new entity to create</param>
    /// <returns>The entity's ID</returns>
    int Create(T item);

    /// <summary>
    /// Reads an entity object by its ID
    /// </summary>
    /// <param name="id">The entity's ID</param>
    /// <returns>The entity</returns>
    T? Read(int id);

    /// <summary>
    /// Reads an entity object by filter
    /// </summary>
    /// <param name="filter">The filter for the read</param>
    /// <returns>The entity</returns>
    T? Read(Func<U, bool> filter); // stage 2

    /// <summary>
    /// Reads all the entities in DAL by filter
    /// </summary>
    /// <param name="filter">The filter for the read</param>
    /// <returns>Iterator to get all the entities</returns>
    IEnumerable<T?> ReadAll(Func<U, bool>? filter = null); // stage 2

    /// <summary>
    /// Updates an entity object
    /// </summary>
    /// <param name="item">The new entity to update</param>
    void Update(T item);

    /// <summary>
    /// Deletes an object by its ID
    /// </summary>
    /// <param name="id">The entity's ID</param>
    void Delete(int id);
}