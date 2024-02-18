using BlApi;

namespace BlImplementation;
internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IMilestone Milestone => new MilestoneImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();

    /// <summary>
    /// The set/get property of the project start date
    /// </summary>
    public DateTime? ProjectStartDate 
    {
        get {  return _dal.ProjectStartDate; } 
        set 
        {
            if (ProjectStartDate is not null)
                throw new InvalidOperationException("There's already date for the project. You can't reset.");
            if (value < DateTime.Now)
                throw new InvalidOperationException("You can't set a past date.");
            if (value is not null)
                _dal.ProjectStartDate = value;
        }
    }

    /// <summary>
    /// Checking the project status according to the stages
    /// </summary>
    /// <returns>Enum of the project status</returns>
    public BO.ProjectStatus CheckProjectStatus()
    {
        if (ProjectStartDate is null)
            return BO.ProjectStatus.Planing;
        else
        {
            if(_dal.Task.ReadAll().All(task => task?.ScheduledDate is not null))
                return BO.ProjectStatus.InProgress;
            else
                return BO.ProjectStatus.MiddlePlaning;
        }
    }
}