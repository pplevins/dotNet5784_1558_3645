namespace BlApi;

using BO;

/// <summary>
/// Interface for the CRUD operations on the Task entity in the Data Access Layer (DAL).
/// </summary>
public interface ITask : ICrud<BO.Task, DO.Task>
{
    public DO.Task UpdateScheduledDate(DO.Task doTask, DateTime? date);
    public DateTime SuggestScheduledDate(int id);
    DateTime? StartDateCreation(Task task);
    DateTime? CompleteDateCreation(Task task);
    public IEnumerable<BO.TaskInList?> ReadAllTaskInList(Func<DO.Task, bool>? filter = null);
}