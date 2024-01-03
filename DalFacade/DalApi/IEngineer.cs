namespace DalApi;

using DO;

/// <summary>
/// Interface for the CRUD operation on the Engineer entity.
/// </summary>
public interface IEngineer
{
    int Create(Engineer item); // Creates a new Engineer entity object in DAL
    Engineer? Read(int id); // Reads an Engineer entity object by its ID
    List<Engineer?> ReadAll(); // Reads all Engineer entity objects (Stage 1 only)
    void Update(Engineer item); // Updates an Engineer entity object
    void Delete(int id); // Deletes an Engineer object by its ID
}
