using BlApi;
using BO;
using DO;
using System.Linq;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;

/// <summary>
/// Implementation of the <see cref="ITask"/> interface to manage tasks in the data source.
/// </summary>
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        Bl bl = new Bl();
        if (bl.CheckProjectStatus() > BO.ProjectStatus.Planing)
            throw new BO.Exceptions.BlUpdateCreateImpossibleException("Cannot create task at this stage of the project.");
        DO.Task doTask = new DO.Task();
        BO.Tools.CopySimilarFields(boTask, doTask);

        try
        {
            if (BO.Tools.ValidatePositiveNumber(boTask.Id)
                && BO.Tools.ValidateNonEmptyString(boTask.Alias)
                )
            {
                int idTask = _dal.Task.Create(doTask);
                CreateDependencies(boTask.Dependencies, idTask);
                return idTask;
            }
            return 0;
        }
        catch (DO.Exceptions.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    private void CreateDependencies(List<BO.TaskInList> dependencies, int idTask)
    {
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

    public void Delete(int id)
    {
        Bl bl = new Bl();
        if (_dal.Dependency.ReadAll().Any<DO.Dependency?>(ed => ed?.PreviousTask == id))
            throw new BO.Exceptions.BlDeletionImpossibleException($"task with id={id} has dependent tasks and cannot be deleted!");
        if (bl.CheckProjectStatus() > BO.ProjectStatus.Planing)
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

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={id} does Not exist");
        return new BO.Task()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Deliverables = doTask.Deliverables,
            DifficultyLevel = (BO.EngineerExperience)doTask.DifficultyLevel,
            Status = CalcStatus(doTask),
            Dependencies = CalcDependencies(id),
            Milestone = null,
            RequiredEffortTime = doTask.RequiredEffortTime,
            CreatedAtDate = doTask.CreatedAtDate,
            Engineer = GetEngineer(doTask.EngineerId),
            Remarks = doTask.Remarks,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            EstimatedDate = new[] { doTask.ScheduledDate + doTask.RequiredEffortTime, doTask.StartDate + doTask.RequiredEffortTime }.Max()
        };
    }

    public BO.Task? Read(Func<DO.Task, bool> filter)
    {
        DO.Task? doTask = _dal.Task.Read(filter);
        if (doTask is null)
            throw new BO.Exceptions.BlDoesNotExistException($"Task does Not exist");
        return new BO.Task()
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Deliverables = doTask.Deliverables,
            DifficultyLevel = (BO.EngineerExperience)doTask.DifficultyLevel,
            Status = CalcStatus(doTask),
            Dependencies = CalcDependencies(doTask.Id),
            Milestone = null,
            RequiredEffortTime = doTask.RequiredEffortTime,
            CreatedAtDate = doTask.CreatedAtDate,
            Engineer = GetEngineer(doTask.EngineerId),
            Remarks = doTask.Remarks,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            EstimatedDate = new[] { doTask.ScheduledDate + doTask.RequiredEffortTime, doTask.StartDate + doTask.RequiredEffortTime }.Max()
        };
    }

    public IEnumerable<BO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll(filter)
                where filter?.Invoke(doTask) ?? true
                orderby doTask.Id
                select new BO.Task
                {
                    Id = doTask.Id,
                    Alias = doTask.Alias,
                    Description = doTask.Description,
                    Deliverables = doTask.Deliverables,
                    DifficultyLevel = (BO.EngineerExperience)doTask.DifficultyLevel,
                    Status = CalcStatus(doTask),
                    Dependencies = CalcDependencies(doTask.Id),
                    Milestone = null,
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    CreatedAtDate = doTask.CreatedAtDate,
                    Engineer = GetEngineer(doTask.EngineerId),
                    Remarks = doTask.Remarks,
                    ScheduledDate = doTask.ScheduledDate,
                    StartDate = doTask.StartDate,
                    DeadlineDate = doTask.DeadlineDate,
                    CompleteDate = doTask.CompleteDate,
                    EstimatedDate = new[] { doTask.ScheduledDate + doTask.RequiredEffortTime, doTask.StartDate + doTask.RequiredEffortTime }.Max()
                });
    }

    public void Update(BO.Task boTask)
    {
        try
        {
            if (BO.Tools.ValidatePositiveNumber(boTask.Id)
                && BO.Tools.ValidateNonEmptyString(boTask.Alias)
                )
            {
                DO.Task doTask = new DO.Task();
                Bl bl = new Bl();
                Expression<Func<DO.Task, object>>[] excludedProperties;
                switch (bl.CheckProjectStatus())
                {
                    case BO.ProjectStatus.Planing:
                        excludedProperties = new Expression<Func<DO.Task, object>>[] { dc => dc.ScheduledDate, dc => dc.CreatedAtDate, dc => dc.StartDate, dc => dc.CompleteDate };
                        BO.Tools.CopySimilarFields(boTask, doTask, null, excludedProperties);
                        break;
                    case BO.ProjectStatus.MiddlePlaning:
                        excludedProperties = new Expression<Func<DO.Task, object>>[] { dc => dc.DifficultyLevel, dc => dc.CreatedAtDate, dc => dc.StartDate, dc => dc.CompleteDate, dc => dc.RequiredEffortTime };
                        BO.Tools.CopySimilarFields(boTask, doTask, null, excludedProperties);
                        doTask = UpdateScheduledDate(doTask, boTask.ScheduledDate);
                        break;
                    case BO.ProjectStatus.InProgress:
                        excludedProperties = new Expression<Func<DO.Task, object>>[] { dc => dc.DifficultyLevel, dc => dc.ScheduledDate, dc => dc.CreatedAtDate, dc => dc.RequiredEffortTime };
                        BO.Tools.CopySimilarFields(boTask, doTask, null, excludedProperties);
                        break;
                }
                if (UpdateDependenciesCheck(boTask.Dependencies, _dal.Dependency.ReadAll().Where(dep => dep.DependentTask == boTask.Id)))
                {
                    DeleteDependencies(boTask.Id);
                    CreateDependencies(boTask.Dependencies, boTask.Id);
                }
                if (boTask.Engineer is not null)
                    doTask = BO.Tools.UpdateEntity(doTask, "EngineerId", boTask.Engineer.Id);
                _dal.Task.Update(doTask);

            }
        }
        catch (DO.Exceptions.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={boTask.Id} doesn't exists", ex);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentNullException(ex.Message);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public DO.Task UpdateScheduledDate(DO.Task doTask, DateTime? date)
    {
        Bl bl = new Bl();
        if (date == null)
            return doTask;
        _dal.Dependency.ReadAll()
            .Where(ed => ed?.DependentTask == doTask.Id)
            .Select(ed => ed?.PreviousTask).ToList()
            .ForEach(taskId =>
            {
                if (bl.ProjectStartDate <= date
                    && _dal.Task.ReadAll()
                    .All(preTask => preTask is not null && preTask.ScheduledDate + preTask.RequiredEffortTime < date))
                    doTask = BO.Tools.UpdateEntity(doTask, "ScheduledDate", date);
                else
                    throw new InvalidOperationException($"Failed to update the ScheduledDate property for task id={doTask.Id} because not all Previous Tasks has dates or the ScheduledDate is earlier.");
            });
        return doTask;
    }

    private BO.TaskStatus CalcStatus(DO.Task? task)
    {
        if (task.CompleteDate.HasValue)
            return BO.TaskStatus.Done;
        if (task.EngineerId.HasValue)
            return BO.TaskStatus.Started;
        if (task.ScheduledDate.HasValue)
            return BO.TaskStatus.Scheduled;
        return BO.TaskStatus.Unscheduled;
    }

    private List<BO.TaskInList> CalcDependencies(int id)
    {
        return (from DO.Dependency doDep in _dal.Dependency.ReadAll()
                where doDep.DependentTask == id
                from DO.Task doTask in _dal.Task.ReadAll()
                where doTask.Id == doDep.PreviousTask
                select new BO.TaskInList
                {
                    Id = doDep.PreviousTask,
                    Alias = doTask.Alias,
                    Description = doTask.Description,
                    Status = CalcStatus(doTask)
                }).ToList();
    }

    private BO.EngineerInTask? GetEngineer(int? id)
    {
        return _dal.Engineer.ReadAll()
            .Where(doEng => doEng?.Id == id)
            .Select(doEng => new BO.EngineerInTask
            {
                Id = doEng.Id,
                Name = doEng.Name
            }).FirstOrDefault() ?? null;

    }

    private bool UpdateDependenciesCheck(List<BO.TaskInList> dependentTasks, IEnumerable<DO.Dependency> dependencies)
    {
        if (dependentTasks is null && dependencies is null)
            return false;
        var depToUpadte = dependentTasks?.Select(dep => dep.Id);
        var depToCheck = dependencies.Select(dep => dep.DependentTask);

        if (depToCheck.OrderBy(id => id).SequenceEqual(depToUpadte.OrderBy(id => id)))
            return false;
        else
            return true;
    }

    private void DeleteDependencies(int id)
    {
        _dal.Dependency.ReadAll()
                .Where(dep => dep?.DependentTask == id)
                .ToList()
                .ForEach(dep => _dal.Dependency.Delete(dep.Id));
    }
}

