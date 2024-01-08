using DalApi;
using DO;

namespace Dal;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        //Strict(regular) Deletion with proper Exception in case of error
        DeletionHelper.StrictDelete(DataSource.Engineers, Read, id);
    }

    public Engineer? Read(int id)
    {
        // Find and return the Engineer with the specified ID or null if not found
        return DataSource.Engineers.FirstOrDefault(d => d.Id == id);
    }

    public List<Engineer?> ReadAll()
    {
        // Return a new list containing copies of all Engineers directly as Engineer?
        return DataSource.Engineers.Select(engineer => Read(engineer.Id)).ToList();
    }

    public void Update(Engineer item)
    {
        // Find the existing Engineer in the list by ID
        var existingEngineerIndex = DataSource.Engineers.FindIndex(d => d.Id == item.Id);

        if (existingEngineerIndex != -1)
        {
            // Replace the existing Dependency in the list
            DataSource.Engineers[existingEngineerIndex] = item;
        }
        else throw new Exception($"Engineer with ID={item.Id} does Not exist");
    }
}
