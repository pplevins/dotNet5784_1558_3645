using DalApi;

namespace Dal;

public class TaskImplementation : ITask
{
    public int Create(DO.Task item)
    {
        int id = DataSource.Config.NextTaskId;
        DO.Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public DO.Task? Read(int id)
    {
        // Find and return the Task with the specified ID or null if not found
        return DataSource.Tasks.FirstOrDefault(d => d.Id == id);
    }

    public List<DO.Task?> ReadAll()
    {
        // Return a new list containing copies of all Tasks directly as Task?
        return DataSource.Tasks.Select(dependency => Read(dependency.Id)).ToList();
    }

    public void Update(DO.Task item)
    {
        // Find the Task in the list by ID
        var existingTaskIndex = DataSource.Tasks.FindIndex(d => d.Id == item.Id);

        if (existingTaskIndex != -1)
        {
            // Replace the existing Task in the list
            DataSource.Tasks[existingTaskIndex] = item;
        }
        else throw new Exception($"Task with ID={item.Id} does Not exist");
    }
}
