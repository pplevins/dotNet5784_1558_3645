using DalApi;
using DO;

namespace Dal;
/// <summary>
/// Implementation of the <see cref="IEngineer"/> interface to manage engineers in the data source.
/// </summary>
public class EngineerImplementation : IEngineer
{
    /// <inheritdoc />
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");

        DataSource.Engineers.Add(item);
        return item.Id;
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        //regular Deletion with proper Exception in case of error
        var existingItem = Read(id) ?? throw new Exception($"Engineer with ID={id} does not exist");
        DataSource.Engineers.Remove(existingItem);
    }

    /// <inheritdoc />
    public Engineer? Read(int id)
    {
        // Find and return the Engineer with the specified ID or null if not found
        return DataSource.Engineers.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public List<Engineer?> ReadAll()
    {
        // Return a new list containing copies of all Engineers directly as Engineer?
        return DataSource.Engineers.Select(engineer => Read(engineer.Id)).ToList();
    }

    /// <inheritdoc />
    public void Update(Engineer item)
    {
        // Find the existing Engineer in the list by ID
        var existingEngineerIndex = DataSource.Engineers.FindIndex(d => d.Id == item.Id);

        if (existingEngineerIndex != -1)
        {
            // Replace the existing Engineer in the list
            DataSource.Engineers[existingEngineerIndex] = item;
        }
        else
        {
            throw new Exception($"Engineer with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Clear the list of engineers
        DataSource.Engineers.Clear();
    }
}
