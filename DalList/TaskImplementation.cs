using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using static DO.Exceptions;

namespace Dal;

/// <summary>
/// Implementation of the <see cref="ITask"/> interface to manage tasks in the data source.
/// </summary>
internal class TaskImplementation : ITask
{
    private readonly ICreationStrategy<DO.Task> _creationStrategy;
    private readonly IDeletionStrategy<DO.Task> _deletionStrategy;

    public TaskImplementation()
    {
        //Internal id creation(running id) so we don't get the id in the entity itself, but we need to provide a method for the creation of it
        _creationStrategy = new InternalIdCreationStrategy<DO.Task>(idGenerator: () => DataSource.Config.NextTaskId);
        //soft Deletion(only marks the entity as non-active) with proper Exception in case of error
        _deletionStrategy = new SoftDeletionStrategy<DO.Task>(Read, Update);
    }
    /// <inheritdoc />
    public int Create(DO.Task item)
    {
        return _creationStrategy.Create(item, DataSource.Tasks);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        //regular Deletion with proper Exception in case of error
        _deletionStrategy.Delete(id, DataSource.Tasks);
    }

    /// <inheritdoc />
    public DO.Task? Read(int id)
    {
        // Find and return the Task with the specified ID or null if not found
        //Even when the task is inactive, returns the entity with isActive = false
        return DataSource.Tasks.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null) //stage 2
    {
        //returns all the active tasks
        return DataSource.Tasks.Where(item => item.IsActive && (filter?.Invoke(item) ?? true));
    }

    /// <inheritdoc />
    public void Update(DO.Task item)
    {
        // Find the Task in the list by ID
        var existingTaskIndex = DataSource.Tasks.FindIndex(d => d.Id == item.Id);

        if (existingTaskIndex != -1)
        {
            // Replace the existing Task in the list
            DataSource.Tasks[existingTaskIndex] = item;
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Clear the list of tasks
        DataSource.Tasks.Clear();
        DataSource.Config.ResetTaskId();
    }
}

