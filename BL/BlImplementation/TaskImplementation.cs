using BlApi;
using BO;
using Task = BO.Task;

namespace BlImplementation;

/// <summary>
/// Implementation of the <see cref="ITask"/> interface to manage tasks in the data source.
/// </summary>
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;

    /// <summary>
    /// Creates new task in BL
    /// </summary>
    /// <param name="boTask">Bl task entity</param>
    /// <returns>id of the task</returns>
    /// <exception cref="BO.Exceptions.BlUpdateCreateImpossibleException">if the project is not in the suitable stage to create new task</exception>
    /// <exception cref="BO.Exceptions.BlAlreadyExistsException">In case the task already exists</exception>
    /// <exception cref="ArgumentException">in case the input is not valid</exception>
    public int Create(BO.Task boTask)
    {
        if (_bl.CheckProjectStatus() > BO.ProjectStatus.Planing)
            throw new BO.Exceptions.BlUpdateCreateImpossibleException("Cannot create task at this stage of the project.");
        DO.Task doTask = new DO.Task() { CreatedAtDate = _bl.Clock };
        BO.Tools.CopySimilarFields(boTask, doTask);
        try
        {
            ValidateTask(boTask);
            int idTask = _dal.Task.Create(doTask);
            CreateDependencies(boTask.Dependencies, idTask);
            if (boTask.Engineer is not null)
                doTask = UpdateEngineerInTask(doTask, boTask.Engineer.Id);
            return idTask;
        }
        catch (DO.Exceptions.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Create new dependencies in DAL according to the list in task entity
    /// </summary>
    /// <param name="dependencies">the list of previous tasks</param>
    /// <param name="idTask">id of the dependent task</param>
    private void CreateDependencies(List<BO.TaskInList> dependencies, int idTask)
    {
        if (dependencies is null)
            return;
        foreach (var depTask in dependencies)
        {
            DO.Dependency dependency = new()
            {
                DependentTask = idTask,
                PreviousTask = depTask.Id
            };
            _dal.Dependency.Create(dependency);
        }
    }

    /// <summary>
    /// Deletes task in BL
    /// </summary>
    /// <param name="id">id of the task to delete</param>
    /// <exception cref="BO.Exceptions.BlDeletionImpossibleException">in case there's a previous tasks or can't delete in this project stage</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the task does not exist</exception>
    public void Delete(int id)
    {
        if (_dal.Dependency.ReadAll().Any<DO.Dependency?>(ed => ed?.PreviousTask == id))
            throw new BO.Exceptions.BlDeletionImpossibleException($"task with id={id} has dependent tasks and cannot be deleted!");
        if (_bl.CheckProjectStatus() > BO.ProjectStatus.Planing)
            throw new BO.Exceptions.BlDeletionImpossibleException("You can't delete task at this stage of the project");
        try
        {
            _dal.Task.Delete(id);
            DeleteDependencies(id);
        }
        catch (DO.Exceptions.DalDeletionImpossibleException ex)
        {
            throw new BO.Exceptions.BlDeletionImpossibleException("Task not found", ex);
        }
        catch (DO.Exceptions.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Task with id={id} does not exist", ex);
        }
    }

    /// <summary>
    /// Reads a task
    /// </summary>
    /// <param name="id">id of the task to read</param>
    /// <returns>BO.task entity</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the task does not exist</exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={id} does Not exist");
        BO.Tools.CheckActive("Task", doTask);
        return ConvertFromDoToBoTask(doTask);
    }

    /// <summary>
    /// Reads an task according to filter
    /// </summary>
    /// <param name="filter">filter to the read function</param>
    /// <returns>BO.task entity</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the task does not exist</exception>
    public BO.Task? Read(Func<DO.Task, bool> filter)
    {
        DO.Task? doTask = _dal.Task.Read(filter);
        if (doTask is null)
            throw new BO.Exceptions.BlDoesNotExistException($"Task does Not exist");
        BO.Tools.CheckActive("Task", doTask);
        return ConvertFromDoToBoTask(doTask);
    }

    /// <summary>
    /// Reads all tasks
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns>IEnumerable of all tasks</returns>
    public IEnumerable<BO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll(filter)
                where filter?.Invoke(doTask) ?? true
                orderby doTask.Id
                select ConvertFromDoToBoTask(doTask));
    }


    /// <summary>
    /// Updates task in Bl
    /// </summary>
    /// <param name="boTask">BO.task entity to update</param>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the task does not exist</exception>
    public void Update(BO.Task boTask)
    {
        try
        {
            ValidateTask(boTask);
            DO.Task? doTask = _dal.Task.Read(boTask.Id);
            doTask = UpdateFieldsBasedOnProjectStatus(boTask, doTask);
            UpdateDependencies(boTask);
            UpdateEngineerAssignment(boTask?.Engineer?.Id, doTask);

            _dal.Task.Update(doTask);
        }
        catch (DO.Exceptions.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={boTask.Id} doesn't exists", ex);
        }
    }

    /// <summary>
    /// Validation for task. throw exception if failed
    /// </summary>
    /// <param name="boTask">Task to validate</param>
    private void ValidateTask(BO.Task boTask)
    {
        BO.Tools.ValidatePositiveNumber<int>(boTask.Id);
        BO.Tools.ValidateNonEmptyString(boTask.Alias);
    }

    /// <summary>
    /// Check the status of the project and excluding the properties accordingly.
    /// </summary>
    /// <param name="boTask">BO.Task entity</param>
    /// <param name="doTask">DO.Task entity</param>
    /// <returns>the DO.Task entity with the updated dields.</returns>
    /// <exception cref="InvalidOperationException">in case the status is not correct.</exception>
    private DO.Task? UpdateFieldsBasedOnProjectStatus(BO.Task boTask, DO.Task? doTask)
    {
        string[] excludedProperties;
        BO.ProjectStatus projectStatus = _bl.CheckProjectStatus();

        switch (projectStatus)
        {
            case BO.ProjectStatus.Planing:
                excludedProperties = new[] { nameof(DO.Task.ScheduledDate), nameof(DO.Task.CreatedAtDate), nameof(DO.Task.StartDate), nameof(DO.Task.CompleteDate) };
                doTask = BO.Tools.UpdateEntity(doTask, "DifficultyLevel", (DO.EngineerExperience)boTask.DifficultyLevel);
                break;
            case BO.ProjectStatus.MiddlePlaning:
                excludedProperties = new[] { nameof(DO.Task.DifficultyLevel), nameof(DO.Task.CreatedAtDate), nameof(DO.Task.StartDate), nameof(DO.Task.CompleteDate), nameof(DO.Task.RequiredEffortTime) };
                doTask = UpdateScheduledDate(doTask, boTask.ScheduledDate);
                break;
            case BO.ProjectStatus.InProgress:
                excludedProperties = new[] { nameof(DO.Task.DifficultyLevel), nameof(DO.Task.ScheduledDate), nameof(DO.Task.CreatedAtDate), nameof(DO.Task.RequiredEffortTime) };
                break;
            default:
                throw new InvalidOperationException("Invalid project status.");
        }

        BO.Tools.CopySimilarFields(boTask, doTask, null, excludedProperties);
        return doTask;
    }

    /// <summary>
    /// updates the dependencies in task.
    /// </summary>
    /// <param name="boTask">BO.Task entity</param>
    private void UpdateDependencies(BO.Task boTask)
    {
        bool needChange = UpdateDependenciesCheck(boTask.Dependencies, _dal.Dependency.ReadAll().Where(dep => dep.DependentTask == boTask.Id));
        if (needChange)
        {
            DeleteDependencies(boTask.Id);
            CreateDependencies(boTask.Dependencies, boTask.Id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="engineerId"></param>
    /// <param name="doTask"></param>
    /// <returns></returns>
    /// <exception cref="BO.Exceptions.BlUpdateCreateImpossibleException"></exception>
    private DO.Task? UpdateEngineerAssignment(int? engineerId, DO.Task? doTask)
    {
        if (engineerId is not null)
        {
            if (_bl.CheckProjectStatus() < BO.ProjectStatus.InProgress)
                throw new BO.Exceptions.BlUpdateCreateImpossibleException("Cannot assign engineer to task at this stage of the project.");

            doTask = UpdateEngineerInTask(doTask, engineerId.Value);
            doTask = BO.Tools.UpdateEntity(doTask, nameof(DO.Task.StartDate), _bl.Clock);
        }
        return doTask;
    }



    /// <summary>
    /// Update the ScheduledDate property in task
    /// </summary>
    /// <param name="doTask">DO.Task to update</param>
    /// <param name="date">the ScheduledDate value to update</param>
    /// <returns>the updated DO.Task</returns>
    /// <exception cref="InvalidOperationException">in case previous tasks doesn't have a date</exception>
    public DO.Task UpdateScheduledDate(DO.Task doTask, DateTime? date)
    {
        if (date == null)
            return doTask;
        _dal.Dependency.ReadAll()
            .Where(ed => ed?.DependentTask == doTask.Id)
            .Select(ed => ed?.PreviousTask).ToList()
            .ForEach(taskId =>
            {
                DO.Task? task;
                task = _dal.Task.Read((int)taskId!);
                if (_bl.ProjectStartDate <= date && task.ScheduledDate + task.RequiredEffortTime <= date)
                    doTask = BO.Tools.UpdateEntity(doTask, nameof(DO.Task.ScheduledDate), date);
                else
                    throw new InvalidOperationException($"Failed to update the ScheduledDate property for task id={doTask.Id} because not all Previous Tasks has dates or the ScheduledDate is earlier.");
            });
        return doTask;
    }

    /// <summary>
    /// Calculing the status of a task
    /// </summary>
    /// <param name="task">the DO.Task to calc</param>
    /// <returns>the status enum</returns>
    private BO.TaskStatus CalcStatus(DO.Task? task)
    {
        if (task.CompleteDate.HasValue)
            return BO.TaskStatus.Done;
        if (task.EngineerId.HasValue && task.StartDate.HasValue)
            return BO.TaskStatus.Started;
        if (task.ScheduledDate.HasValue)
            return BO.TaskStatus.Scheduled;
        return BO.TaskStatus.Unscheduled;
    }

    /// <summary>
    /// Calculing the dependencies of each task
    /// </summary>
    /// <param name="id">id of the task to calc</param>
    /// <returns>the list of previous tasks</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the task doesn't exist</exception>
    private List<BO.TaskInList> CalcDependencies(int id)
    {
        return (from dep in _dal.Dependency.ReadAll()
                where dep?.DependentTask == id
                let preId = dep.PreviousTask
                let doTask = _dal.Task.Read(preId) ?? throw new BO.Exceptions.BlDoesNotExistException($"Task with id={preId} doesn't exist, and can't be the previous task")
                select new BO.TaskInList
                {
                    Id = doTask.Id,
                    Alias = doTask.Alias,
                    Description = doTask.Description,
                    Status = CalcStatus(doTask)
                }).ToList();
    }

    /// <summary>
    /// Gets the engineer details for engineerInTask
    /// </summary>
    /// <param name="id">id of the engineer</param>
    /// <returns>EngineerInTask entity</returns>
    public BO.EngineerInTask? GetEngineer(int? id)
    {
        return _dal.Engineer.ReadAll()
            .Where(doEng => doEng?.Id == id)
            .Select(doEng => new BO.EngineerInTask
            {
                Id = doEng.Id,
                Name = doEng.Name
            }).FirstOrDefault() ?? null;

    }

    /// <summary>
    /// Updates EngineerInTask in task
    /// </summary>
    /// <param name="doTask">DO.Task to update</param>
    /// <param name="id">engineer id</param>
    /// <returns>the updated DO.Task</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the engineer does not exist</exception>
    /// <exception cref="InvalidOperationException">in case there's logical problem to assign engineer to the task</exception>
    private DO.Task UpdateEngineerInTask(DO.Task doTask, int id)
    {
        DO.Engineer? eng = _dal.Engineer.Read(id);
        if (eng == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with id={id} does not exist and cannot be assigned to the task");
        if (doTask.DifficultyLevel > eng.Level)
            throw new InvalidOperationException($"Engineer with level={eng.Level} cannot be assigned to task with level {doTask.DifficultyLevel}");
        _dal.Dependency.ReadAll()
            .Where(dep => dep.DependentTask == doTask.Id)
            .Select(dep => dep.PreviousTask).ToList()
            .ForEach(preId =>
            {
                DO.Task? task = _dal.Task.Read(preId);
                if (CalcStatus(task) != BO.TaskStatus.Done)
                    throw new InvalidOperationException("You can't assign engineer to task with undone previous tasks.");
            });
        return BO.Tools.UpdateEntity(doTask, nameof(DO.Task.EngineerId), id);
    }

    /// <summary>
    /// Gets list of the previos tasks
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    public List<BO.Task> GetDependentTasks(BO.Task boTask)
    {
        var dependentTasks = _dal.Dependency.ReadAll()
            .Where(dep => dep.DependentTask == boTask.Id) // Filter dependent tasks for the given task ID
            .Select(dep => dep.PreviousTask)
            .Join(_dal.Task.ReadAll(),
                prevTaskId => prevTaskId,
                task => task.Id,
                (prevTaskId, task) => task)
            .Select(doT => ConvertFromDoToBoTask(doT))
            .ToList();

        return dependentTasks;
    }

    /// <summary>
    /// Converts DO task entity to BO task entity, and arrange the appropriate properties
    /// </summary>
    /// <param name="doTask">DO task to convert</param>
    /// <returns>converted BO task</returns>
    private BO.Task ConvertFromDoToBoTask(DO.Task doTask)
    {
        var boTask = new BO.Task()
        {
            DifficultyLevel = (BO.EngineerExperience)doTask.DifficultyLevel,
            Status = CalcStatus(doTask),
            Dependencies = CalcDependencies(doTask.Id),
            Engineer = GetEngineer(doTask.EngineerId),
            EstimatedDate = new[] { doTask.ScheduledDate + doTask.RequiredEffortTime, doTask.StartDate + doTask.RequiredEffortTime }.Max()
        };
        BO.Tools.CopySimilarFields(doTask, boTask);
        return boTask;
    }

    /// <summary>
    /// Converts BO task entity to DO task entity, and arrange the appropriate properties
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    private DO.Task ConvertFromBoToDoTask(BO.Task boTask)
    {
        var doTask = new DO.Task();
        BO.Tools.CopySimilarFields(boTask, doTask);
        doTask = BO.Tools.UpdateEntity(doTask, nameof(DO.Task.DifficultyLevel), (BO.EngineerExperience)doTask.DifficultyLevel);
        return doTask;
    }

    /// <summary>
    /// Checks if task has a previos tasks that didn't finished.
    /// </summary>
    /// <param name="doTask">The dependent task.</param>
    /// <param name="dependentTasks">the previous tasks list.</param>
    /// <returns>true if there's a task that didn't finished.</returns>
    public bool CheckForActiveDependentTasks(BO.Task doTask, List<BO.Task> dependentTasks)
    {
        return dependentTasks.Any(task => task?.CompleteDate is null);
    }

    /// <summary>
    /// Gets suitable tasks for engineer to work on.
    /// </summary>
    /// <param name="engineerId">ID of the working engineer.</param>
    /// <returns>list of the suitable tasks.</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    public List<BO.TaskInList> GetSuitableTasks(int engineerId)
    {
        DO.Engineer? eng = _dal.Engineer.Read(engineerId);
        if (eng is null)
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with id={engineerId} does not exist and cannot be assigned to the task");
        var tasks = _dal.Task.ReadAll();
        List<Task> suitableTasks = new List<Task>();
        foreach (var doTask in tasks)
        {
            if (doTask.DifficultyLevel <= eng.Level && doTask.EngineerId == null)
            {
                var boTask = ConvertFromDoToBoTask(doTask);
                var dependentTasks = GetDependentTasks(boTask);
                if (!CheckForActiveDependentTasks(boTask, dependentTasks)) suitableTasks.Add(boTask);
            }
        }
        return ConvertFromTaskToTaskList(suitableTasks);
    }

    /// <summary>
    /// converts list of task to list of TaskInList for presentation.
    /// </summary>
    /// <param name="tasks">list of tasks.</param>
    /// <returns>list of TaskInList.</returns>
    public List<BO.TaskInList> ConvertFromTaskToTaskList(List<BO.Task> tasks)
    {
        {
            return tasks.Select(task => new BO.TaskInList
            {
                Id = task.Id,
                Alias = task.Alias,
                Description = task.Description,
                Status = CalcStatus(ConvertFromBoToDoTask(task))
            }).ToList();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    /// <param name="depId"></param>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void addDependency(BO.Task task, int depId)
    {
        if (!_bl.Task.ReadAll().Any(dep => dep.Id == depId))
            throw new BO.Exceptions.BlDoesNotExistException($"Task with id={depId} does not exist");
        if (_dal.Dependency.ReadAll().Any(dep => dep.DependentTask == depId && dep.PreviousTask == task.Id))
            throw new InvalidOperationException($"Can't depend task id={task.Id} on task id={depId} because a circular dependency is created");
        List<BO.TaskInList> dependencies = CalcDependencies(task.Id);
        BO.Task depTask = Read(depId);
        BO.TaskInList newDep = new BO.TaskInList()
        {
            Id = depId,
            Alias = depTask.Alias,
            Description = depTask.Description,
            Status = depTask.Status
        };
        dependencies.Add(newDep);
        task.Dependencies = dependencies;
    }

    /// <summary>
    /// Gets a list of task that can be dependent on given task.
    /// </summary>
    /// <param name="taskId">the id of the given task</param>
    /// <returns>list of task to choose dependency.</returns>
    public List<TaskInList> CalcDependenciesToChoose(int taskId)
    {
        List<TaskInList> depsToAdd = new List<TaskInList>();
        _bl.Task.ReadAll().ToList().ForEach(task =>
        {
            if (_dal.Dependency.ReadAll().Any(dep => dep.DependentTask == taskId && dep.PreviousTask == task.Id)) { }
            else
            {
                BO.TaskInList taskInList = new BO.TaskInList()
                {
                    Id = task.Id,
                    Alias = task.Alias,
                    Description = task.Description,
                    Status = task.Status
                };
                depsToAdd.Add(taskInList);
            }
        });
        return depsToAdd;
    }

    /// <summary>
    /// Check if the TaskInList list from the user is need to update
    /// </summary>
    /// <param name="dependentTasks">the TaskInList list</param>
    /// <param name="dependencies">the IEnumerable of the previous tasks</param>
    /// <returns>true if there's difference between the previous tasks</returns>
    private bool UpdateDependenciesCheck(List<BO.TaskInList> dependentTasks, IEnumerable<DO.Dependency> dependencies)
    {
        if (dependentTasks is null && dependencies is null)
            return false;
        if (dependentTasks is null || dependencies is null)
            return true;
        var depToUpadte = dependentTasks?.Select(dep => dep.Id);
        var depToCheck = dependencies.Select(dep => dep.PreviousTask);

        if (depToCheck.OrderBy(id => id).SequenceEqual(depToUpadte?.OrderBy(id => id)))
            return false;
        else
            return true;
    }

    /// <summary>
    /// deletes dpendencies of task
    /// </summary>
    /// <param name="id">id  of the dependent task</param>
    private void DeleteDependencies(int id)
    {
        _dal.Dependency.ReadAll()
                .Where(dep => dep?.DependentTask == id)
                .ToList()
                .ForEach(dep => _dal.Dependency.Delete(dep.Id));
    }

    /// <summary>
    /// Suggest ScheduledDate for task
    /// </summary>
    /// <param name="id">task is</param>
    /// <returns>the date Suggested</returns>
    /// <exception cref="ArgumentException">in case there's previous tasks without ScheduledDate</exception>
    /// <exception cref="InvalidOperationException">in case trying to enter dates to tasks in planing project stage</exception>
    public DateTime SuggestScheduledDate(int id)
    {
        if (_bl.CheckProjectStatus() == ProjectStatus.Planing)
            throw new InvalidOperationException("You can't enter dates to tasks, before entering start date of the project");
        List<BO.TaskInList> depList = CalcDependencies(id);
        if (depList.Count == 0)
            return (DateTime)_bl.ProjectStartDate!;
        if (depList.Any(dep => dep.Status == BO.TaskStatus.Unscheduled))
            throw new ArgumentException("There's at least one previous task without Schedule Date");
        return (DateTime)_dal.Task.ReadAll()
            .Where(task => depList.Any(dep => dep.Id == task!.Id))
            .Select(task => task!.ScheduledDate + task.RequiredEffortTime)
            .Max()!;
    }

    /// <summary>
    /// StartDateCreation for task
    /// </summary>
    /// <returns>the date Suggested</returns>
    public DateTime? StartDateCreation(Task task)
    {
        if (_bl.CheckProjectStatus() == ProjectStatus.Planing)
            throw new InvalidOperationException("You can't enter dates to tasks, before entering start date of the project");
        task.StartDate = _bl.Clock;
        task.EstimatedDate = CalculateEstimatedDate(task);
        Update(task);
        return task.StartDate;
    }

    /// <summary>
    /// StartDateCreation for task
    /// </summary>
    /// <returns>the date Suggested</returns>
    public DateTime? StartDateCreation(int taskId, int engineerId)
    {
        if (_bl.CheckProjectStatus() == ProjectStatus.Planing)
            throw new InvalidOperationException("You can't enter dates to tasks, before entering start date of the project");
        var task = Read(taskId);
        task.Engineer = GetEngineer(engineerId);
        task.StartDate = _bl.Clock;
        task.EstimatedDate = CalculateEstimatedDate(task);
        Update(task);
        return task.StartDate;
    }

    /// <summary>
    /// CompleteDateCreation for task
    /// </summary>
    /// <returns>the complete date</returns>
    public DateTime? CompleteDateCreation(Task task)
    {
        if (_bl.CheckProjectStatus() == ProjectStatus.Planing)
            throw new InvalidOperationException("You can't enter dates to tasks, before entering start date of the project");
        task.CompleteDate = _bl.Clock;
        Update(task);
        return task.CompleteDate;
    }

    /// <summary>
    /// CompleteDateCreation for task
    /// </summary>
    /// <returns>the complete date</returns>
    public int CompleteDateAction(int taskId)
    {
        var task = Read(taskId);
        if (_bl.CheckProjectStatus() == ProjectStatus.Planing)
            throw new InvalidOperationException("You can't enter dates to tasks, before entering start date of the project");
        task.CompleteDate = _bl.Clock;
        Update(task);
        return task.Id;
    }

    /// <summary>
    /// Calculate estimated date for task
    /// </summary>
    /// <returns>the estimated date</returns>
    public DateTime? CalculateEstimatedDate(Task task)
    {
        return new[] { task.ScheduledDate + task.RequiredEffortTime, task.StartDate + task.RequiredEffortTime }.Max();
    }

    /// <summary>
    /// reseting all the task and dependency database.
    /// </summary>
    public void Reset()
    {
        _dal.Task.Reset();
        _dal.Dependency.Reset();
    }

    /// <summary>
    /// Read all TaskInList for the PL.
    /// </summary>
    /// <param name="filter">filter to apply.</param>
    /// <returns>list of TaskInList.</returns>
    public IEnumerable<BO.TaskInList?> ReadAllTaskInList(Func<DO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll(filter)
                where filter?.Invoke(doTask) ?? true
                orderby doTask.Id
                select new BO.TaskInList
                {
                    Id = doTask.Id,
                    Alias = doTask.Alias,
                    Description = doTask.Description,
                    Status = CalcStatus(doTask)
                });
    }
}