using BlApi;
using BO;

namespace BlImplementation;
/// <summary>
/// Implementation of the <see cref="IEngineer"/> interface to manage engineers in the data source.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private IBl bl = Factory.Get();

    /// <summary>
    /// Creates new engineer in BL
    /// </summary>
    /// <param name="boEngineer">Bl engineer entity</param>
    /// <returns>id of the engineer</returns>
    /// <exception cref="BO.Exceptions.BlAlreadyExistsException">In case the engineer already exists</exception>
    /// <exception cref="ArgumentException">in case the input is not valid</exception>
    public int Create(Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer();
        BO.Tools.CopySimilarFields(boEngineer, doEngineer);
        doEngineer = BO.Tools.UpdateEntity(doEngineer, "Level", (DO.EngineerExperience)boEngineer.Level);
        try
        {
            if (BO.Tools.ValidatePositiveNumber<int>(boEngineer.Id) 
                && BO.Tools.ValidateNonEmptyString(boEngineer.Name) 
                && BO.Tools.ValidateEmailAddress(boEngineer.Email) 
                && BO.Tools.ValidatePositiveNumber<double>(boEngineer.Cost)
                )
            {
                int idEng = _dal.Engineer.Create(doEngineer);
                return idEng;
            }
            return 0;
        }
        catch (DO.Exceptions.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    /// <summary>
    /// Deletes engineer in BL
    /// </summary>
    /// <param name="id">id of the engineer to delete</param>
    /// <exception cref="BO.Exceptions.BlDeletionImpossibleException">in case the engineer is working on a task</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the engineer does not exist</exception>
    public void Delete(int id)
    {
        if (_dal.Task.ReadAll().Any<DO.Task?>(ed => ed?.EngineerId == id && ed.CompleteDate == null))
            throw new BO.Exceptions.BlDeletionImpossibleException($"Engineer with id={id} is working on task and cannot be deleted!");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.Exceptions.DalDeletionImpossibleException ex)
        {
            throw new BO.Exceptions.BlDeletionImpossibleException("Engineer not found", ex);
        }
        catch (DO.Exceptions.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with id={id} does not exist", ex);
        }
    }

    /// <summary>
    /// Reads an engineer
    /// </summary>
    /// <param name="id">id of the engineer</param>
    /// <returns>BO.engineer entity</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the engineer does not exist</exception>
    public Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        BO.Tools.CheckActive("Engineer", doEngineer);

        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = (from DO.Task doTask in _dal.Task.ReadAll()
                    where doTask.EngineerId == id && doTask.CompleteDate == null
                    select new BO.TaskInEngineer
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias
                    }).FirstOrDefault()
        };
    }

    /// <summary>
    /// Reads an engineer according to filter
    /// </summary>
    /// <param name="filter">filter to the read function</param>
    /// <returns>BO.engineer entity</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the engineer does not exist</exception>
    public Engineer? Read(Func<DO.Engineer, bool> filter)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(filter);
        if (doEngineer == null)
            throw new BO.Exceptions.BlDoesNotExistException("Enginner does Not exist");

        BO.Tools.CheckActive("Engineer", doEngineer);
        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = (from DO.Task doTask in _dal.Task.ReadAll()
                    where doTask.EngineerId == doEngineer.Id && doTask.CompleteDate == null
                    select new BO.TaskInEngineer
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias
                    }).FirstOrDefault()
        };
    }

    /// <summary>
    /// Reads all engineers
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns>IEnumerable of all engineers</returns>
    public IEnumerable<Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                where filter?.Invoke(doEngineer) ?? true
                orderby doEngineer.Id
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience)doEngineer.Level,
                    Cost = doEngineer.Cost,
                    Task = (from DO.Task doTask in _dal.Task.ReadAll()
                            where doTask.EngineerId == doEngineer.Id && doTask.CompleteDate == null
                            select new BO.TaskInEngineer
                            {
                                Id = doTask.Id,
                                Alias = doTask.Alias
                            }).FirstOrDefault()
                });
    }

    public void Reset()
    {
        _dal.Engineer.Reset();
    }

    /// <summary>
    /// Updates engineer in Bl
    /// </summary>
    /// <param name="boEngineer">BO.engineer entity to update</param>
    /// <exception cref="InvalidOperationException">in case some operation is invalid</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">in case the engineer does not exist</exception>
    /// <exception cref="ArgumentException">in case some argument is not valid</exception>
    public void Update(Engineer boEngineer)
    {
        try
        {
            DO.Engineer doEngineer = new DO.Engineer();
            BO.Tools.CopySimilarFields(boEngineer, doEngineer);
            DO.EngineerExperience? doLevel = _dal.Engineer.Read(boEngineer.Id)?.Level;
            if (doLevel is not null && doLevel > (DO.EngineerExperience)boEngineer.Level)
                throw new InvalidOperationException($"Can't demote an engineer. The engineer level can only be updated from {doLevel} and above");
            //integrity check
            if (BO.Tools.ValidatePositiveNumber<int>(boEngineer.Id)
                    && BO.Tools.ValidateNonEmptyString(boEngineer.Name)
                    && BO.Tools.ValidateEmailAddress(boEngineer.Email)
                    && BO.Tools.ValidatePositiveNumber<double>(boEngineer.Cost)
                    )
            {
                doEngineer = BO.Tools.UpdateEntity(doEngineer, "Level", (DO.EngineerExperience)boEngineer.Level);
                if (boEngineer.Task is not null)
                {
                    if (bl.CheckProjectStatus() < BO.ProjectStatus.InProgress)
                        throw new InvalidOperationException("Can't assign engineer to task at this stage of the project");

                    DO.Task? doTask = _dal.Task.Read(boEngineer.Task.Id);
                    if (doTask is null)
                        throw new DO.Exceptions.DalDoesNotExistException($"There's no task with id={boEngineer.Task.Id}");
                    if (doTask.DifficultyLevel > doEngineer.Level)
                        throw new InvalidOperationException($"Can't assign task with level={doTask.DifficultyLevel} to engineer with level={doEngineer.Level}");
                    _dal.Dependency.ReadAll()
                        .Where(dep => dep.DependentTask == doTask.Id)
                        .Select(dep => dep.PreviousTask)
                        .ToList()
                        .ForEach(dep =>
                        {
                            DO.Task? dt = _dal.Task.Read(dep);
                            if (dt is not null && !dt.CompleteDate.HasValue)
                                throw new InvalidOperationException($"Failed to assign engineer to task id={doTask.Id}. Not all previous tast has done yet.");
                        });

                    //setting the DO engineer
                    DO.Task newTask = new DO.Task()
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias,
                        Description = doTask.Description,
                        IsMilestone = doTask.IsMilestone,
                        Deliverables = doTask.Deliverables,
                        DifficultyLevel = doTask.DifficultyLevel,
                        RequiredEffortTime = doTask.RequiredEffortTime,
                        CreatedAtDate = doTask.CreatedAtDate,
                        EngineerId = boEngineer.Task.Id,
                        Remarks = doTask.Remarks,
                        ScheduledDate = doTask.ScheduledDate,
                        StartDate = doTask.StartDate,
                        DeadlineDate = doTask.DeadlineDate,
                        CompleteDate = doTask.CompleteDate,
                        IsActive = doTask.IsActive
                    };
                    _dal.Task.Update(newTask);
                }

                _dal.Engineer.Update(doEngineer);
            }
        }
        catch (DO.Exceptions.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with id={boEngineer.Id} does not exist", ex);
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
}
