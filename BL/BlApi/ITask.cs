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
    DateTime? StartDateCreation(int taskId, int engineerId);
    DateTime? CompleteDateCreation(Task task);
    public IEnumerable<BO.TaskInList?> ReadAllTaskInList(Func<DO.Task, bool>? filter = null);
    public IEnumerable<BO.Task?> ReadAll(Func<DO.Task, bool>? filter = null);
    public List<BO.Task> GetDependentTasks(BO.Task boTask);
    public List<BO.TaskInList?> GetSuitableTasks(int engineerId);
    public int CompleteDateAction(int taskId);
    public List<TaskInList> CalcDependenciesToChoose(int taskId);
    public void addDependency(BO.Task task, int depId);
}