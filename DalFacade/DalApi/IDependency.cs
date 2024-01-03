namespace DalApi;

using DO;

/// <summary>
/// Interface for the CRUD operations on the Dependency entity.
/// </summary>
public interface IDependency
{
    int Create(Dependency item); // Creates a new Dependency entity object in DAL
    Dependency? Read(int id); // Reads a Dependency entity object by its ID
    List<Dependency?> ReadAll(); // Reads all Dependency entity objects (Stage 1 only)
    void Update(Dependency item); // Updates a Dependency entity object
    void Delete(int id); // Deletes a Dependency object by its ID
}
