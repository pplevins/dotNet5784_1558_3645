using DalApi;
using DO;

namespace Dal;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        // Find and return the Dependency with the specified ID or null if not found
        return DataSource.Dependencies.FirstOrDefault(d => d.Id == id);
    }

    public List<Dependency?> ReadAll()
    {
        // Return a new list containing copies of all Dependencies directly as Dependency?
        return DataSource.Dependencies.Select(dependency => Read(dependency.Id)).ToList();
    }

    public void Update(Dependency item)
    {
        // Find the Dependency in the list by ID
        var existingDependencyIndex = DataSource.Dependencies.FindIndex(d => d.Id == item.Id);

        if (existingDependencyIndex != -1)
        {
            // Replace the existing Dependency in the list
            DataSource.Dependencies[existingDependencyIndex] = item;
        }
        else throw new Exception($"Dependency with ID={item.Id} does Not exist");
    }
}
