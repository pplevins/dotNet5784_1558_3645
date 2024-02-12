using BlApi;
using BO;
using System.Collections.Specialized;

namespace BlImplementation;
/// <summary>
/// Implementation of the <see cref="IEngineer"/> interface to manage engineers in the data source.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    
    /// <summary>
    /// Creates new engineer in BL
    /// </summary>
    /// <param name="boEngineer">Bl engineer entity</param>
    /// <returns></returns>
    /// <exception cref="BO.Exceptions.BlAlreadyExistsException">In case the engineer already exists</exception>
    /// <exception cref="ArgumentException">in case the input is not valid</exception>
    public int Create(Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer();
        BO.Tools.CopySimilarFields(boEngineer, doEngineer);
        doEngineer = BO.Tools.UpdateEntity(doEngineer, "Level", (DO.EngineerExperience)boEngineer.Level);
        try
        {
            if (BO.Tools.ValidatePositiveNumber(boEngineer.Id) 
                && BO.Tools.ValidateNonEmptyString(boEngineer.Name) 
                && BO.Tools.ValidateEmailAddress(boEngineer.Email) 
                && BO.Tools.ValidatePositiveNumber(boEngineer.Cost)
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

    public void Delete(int id)
    {
        if(_dal.Task.ReadAll().Any<DO.Task?>(ed => ed?.EngineerId == id))
            throw new BO.Exceptions.BlDeletionImpossibleException($"Engineer with id={id} is working on task and cannot be deleted!");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.Exceptions.DalDeletionImpossibleException ex)
        {
            throw new BO.Exceptions.BlDeletionImpossibleException("Engineer not found", ex);
        }
        catch(DO.Exceptions.DalDoesNotExistException ex) 
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with id={id} does not exist", ex);
        }
    }

    public Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Enginner with ID={id} does Not exist");

        BO.Tools.CheckActive("Engineer", doEngineer);

        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = (from DO.Task doTask in _dal.Task.ReadAll()
                    where doTask.EngineerId == id
                    select new BO.TaskInEngineer
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias
                    }).FirstOrDefault()
        };
    }

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
                    where doTask.EngineerId == doEngineer.Id
                    select new BO.TaskInEngineer
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias
                    }).FirstOrDefault()
        };
    }

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
                            where doTask.EngineerId == doEngineer.Id
                            select new BO.TaskInEngineer
                            {
                                Id = doTask.Id,
                                Alias = doTask.Alias
                            }).FirstOrDefault()
                });
    }

    public void Update(Engineer boEngineer)
    {
        try
        {
            DO.Engineer doEngineer = new DO.Engineer();
            BO.Tools.CopySimilarFields(boEngineer, doEngineer);
            doEngineer = BO.Tools.UpdateEntity(doEngineer, "Level", (DO.EngineerExperience)boEngineer.Level);
            if (BO.Tools.ValidatePositiveNumber(boEngineer.Id)
                    && BO.Tools.ValidateNonEmptyString(boEngineer.Name)
                    && BO.Tools.ValidateEmailAddress(boEngineer.Email)
                    && BO.Tools.ValidatePositiveNumber(boEngineer.Cost)
                    && doEngineer.Level <= (DO.EngineerExperience)boEngineer.Level
                    )
            {
                if (boEngineer.Task is not null)
                {
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
                            if (!dt.CompleteDate.HasValue)
                                throw new InvalidOperationException($"Failed to assign engineer to task id={doTask.Id}. Not all previous tast has done yet.");
                        });
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
    }
}
