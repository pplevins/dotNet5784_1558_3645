using Dal.strategies.create;
using Dal.strategies.delete;
using Dal.Strategies.Create;
using DalApi;
using static DO.Exceptions;

namespace Dal;

/// <summary>
/// Implementation of the <see cref="ITask"/> interface to manage tasks in the data source.
/// </summary>
public class TaskImplementation : ITask
{

    private readonly ICreationStrategy<DO.Task> _creationStrategy;
    private readonly IDeletionStrategy<DO.Task> _deletionStrategy;


    public TaskImplementation()
    {
        _creationStrategy = new InternalIdCreationStrategy<DO.Task>(
            idGenerator: () => DataSource.Config.NextTaskId
        );

        _deletionStrategy = new StrictDeletionStrategy<DO.Task>(Read);
    }
    /// <inheritdoc />
    public int Create(DO.Task item)
    {
        return _creationStrategy.Create(DataSource.Tasks, item);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        //regular Deletion with proper Exception in case of error
        _deletionStrategy.Delete(DataSource.Tasks, id);
    }

    /// <inheritdoc />
    public DO.Task? Read(int id)
    {
        // Find and return the Task with the specified ID or null if not found
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
        return DataSource.Tasks.Where(item => filter?.Invoke(item) ?? true);
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
    }
}

