using Dal.strategies.create;
using Dal.strategies.delete;
using Dal.Strategies.Create;
using DalApi;
using DO;
using static DO.Exceptions;

namespace Dal;

/// <summary>
/// Implementation of the <see cref="IDependency"/> interface to manage dependencies in the data source.
/// </summary>
internal class DependencyImplementation : IDependency
{
    private readonly ICreationStrategy<Dependency> _creationStrategy;
    private readonly IDeletionStrategy<Dependency> _deletionStrategy;

    public DependencyImplementation()
    {
        //Internal id creation(running id) so we don't get the id in the entity itself, but we need to provide a method for the creation of it
        _creationStrategy = new InternalIdCreationStrategy<Dependency>(idGenerator: () => DataSource.Config.NextDependencyId);
        //soft Deletion(only marks the entity as non-active) with proper Exception in case of error
        _deletionStrategy = new SoftDeletionStrategy<Dependency>(Read, Update);
    }
    /// <inheritdoc />
    public int Create(Dependency item)
    {
        return _creationStrategy.Create(DataSource.Dependencies, item);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        _deletionStrategy.Delete(DataSource.Dependencies, id);
    }
    /// <inheritdoc />
    public Dependency? Read(int id)
    {
        // Find and return the Dependency with the specified ID or null if not found
        return DataSource.Dependencies.FirstOrDefault(d => d.Id == id && d.IsActive);
    }
    /// <inheritdoc />
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        return DataSource.Dependencies.Where(item => filter?.Invoke(item) ?? true);
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
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Clear the list of dependencies
        DataSource.Dependencies.Clear();
    }
}

