
using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using static DO.Exceptions;

namespace Dal;
internal class TaskImplementation : ITask
{
    private const string s_tasks_file_name = "tasks";
    private readonly ICreationStrategy<DO.Task> _creationStrategy;
    private readonly IDeletionStrategy<DO.Task> _deletionStrategy;

    public TaskImplementation()
    {
        //Internal id creation(running id) so we don't get the id in the entity itself, but we need to provide a method for the creation of it
        _creationStrategy = new InternalIdCreationStrategy<DO.Task>(idGenerator: () => Config.NextTaskId);
        //soft Deletion(only marks the entity as non-active) with proper Exception in case of error
        _deletionStrategy = new SoftDeletionStrategy<DO.Task>(Read, Update);
    }

    /// <inheritdoc />
    public int Create(DO.Task item)
    {
        return _creationStrategy.Create(item, XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name));
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name);
        _deletionStrategy.Delete(id, tasks, XMLTools.SaveListToXMLSerializer);
    }

    /// <inheritdoc />
    public DO.Task? Read(int id)
    {
        // Find and return the Task with the specified ID or null if not found
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name);
        return tasks.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name);
        return tasks.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null) //stage 3
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name);
        return tasks.Where(item => item.IsActive && (filter?.Invoke(item) ?? true));
    }

    /// <inheritdoc />
    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name);
        // Find the existing Task in the list by ID
        var existingTaskIndex = tasks.FindIndex(d => d.Id == item.Id);

        if (existingTaskIndex != -1)
        {
            // Replace the existing Task in the list
            tasks[existingTaskIndex] = item;
            XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_file_name);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_file_name);
        // Clear the list of Tasks
        tasks.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_file_name);
    }
}
