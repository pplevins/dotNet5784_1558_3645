namespace DalApi;

using DO;

/// <summary>
/// Interface for the CRUD operations on the Task entity in the Data Access Layer (DAL).
/// </summary>
public interface ITask
{
    int Create(Task item); // Creates a new Task entity object in DAL
    Task? Read(int id); // Reads a Task entity object by its ID
    List<Task?> ReadAll(); // Reads all Task entity objects (Stage 1 only)
    void Update(Task item); // Updates a Task entity object
    void Delete(int id); // Deletes a Task object by its ID
}
