using Dal.strategies.create;
using Dal.strategies.delete;
using Dal.Strategies.Create;
using DalApi;
using DO;

namespace Dal;

/// <summary>
/// Implementation of the <see cref="IDependency"/> interface to manage dependencies in the data source.
/// </summary>
public class DependencyImplementation : IDependency
{
    private readonly ICreationStrategy<Dependency> _creationStrategy;
    private readonly IDeletionStrategy<Dependency> _deletionStrategy;


    private readonly Func<int, Dependency, Dependency> getUpdatedItem;
    private readonly Func<int> idGenerator;

    public DependencyImplementation()
    {
        // Initializing _getUpdatedItem with DataSource.Config.NextDependencyId
        getUpdatedItem = (id, item) => item with { Id = DataSource.Config.NextDependencyId };

        // Initializing _idGenerator with a function that creates a new Dependency with the ID set using DataSource.Config.NextDependencyId
        idGenerator = () => DataSource.Config.NextDependencyId;

        _creationStrategy = new InternalIdCreationStrategy<Dependency>(getUpdatedItem, idGenerator);
        _deletionStrategy = new StrictDeletionStrategy<Dependency>();

    }
    /// <inheritdoc />
    public int Create(Dependency item)
    {
        return _creationStrategy.Create(DataSource.Dependencies, item);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        //regular Deletion with proper Exception in case of error
        _deletionStrategy.Delete(DataSource.Dependencies, id, Read);
    }

    /// <inheritdoc />
    public Dependency? Read(int id)
    {
        // Find and return the Dependency with the specified ID or null if not found
        return DataSource.Dependencies.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public List<Dependency?> ReadAll()
    {
        // Return a new list containing copies of all Dependencies directly as Dependency?
        return DataSource.Dependencies.Select(dependency => Read(dependency.Id)).ToList();
    }

    /// <inheritdoc />
    public void Update(Dependency item)
    {
        // Find the Dependency in the list by ID
        var existingDependencyIndex = DataSource.Dependencies.FindIndex(d => d.Id == item.Id);

        if (existingDependencyIndex != -1)
        {
            // Replace the existing Dependency in the list
            DataSource.Dependencies[existingDependencyIndex] = item;
        }
        else
        {
            throw new Exception($"Dependency with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Clear the list of dependencies
        DataSource.Dependencies.Clear();
    }
}

