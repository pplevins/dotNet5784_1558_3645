using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using DO;
using static DO.Exceptions;

namespace Dal;
/// <summary>
/// Implementation of the <see cref="IEngineer"/> interface to manage engineers in the data source.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private readonly ICreationStrategy<Engineer> _creationStrategy;
    private readonly IDeletionStrategy<Engineer> _deletionStrategy;

    public EngineerImplementation()
    {
        //External id creation so we get the id frm outside in the entity itself
        _creationStrategy = new ExternalIdCreationStrategy<Engineer>(Read);
        //soft Deletion(only marks the entity as non-active) with proper Exception in case of error
        _deletionStrategy = new SoftDeletionStrategy<Engineer>(Read, Update);
    }
    /// <inheritdoc />
    public int Create(Engineer item)
    {
        return _creationStrategy.Create(item, DataSource.Engineers);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        //regular Deletion with proper Exception in case of error
        _deletionStrategy.Delete(id, DataSource.Engineers);
    }

    /// <inheritdoc />
    public Engineer? Read(int id)
    {
        // Find and return the Engineer with the specified ID or null if not found.
        //Even when the engineer is inactive, returns the entity with isActive = false
        return DataSource.Engineers.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        //returns all the active engineers
        return DataSource.Engineers.Where(item => item.IsActive && (filter?.Invoke(item) ?? true));
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
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Clear the list of engineers
        DataSource.Engineers.Clear();
    }
}
