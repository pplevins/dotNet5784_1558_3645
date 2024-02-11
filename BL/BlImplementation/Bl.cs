using BlApi;

namespace BlImplementation;
internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IMilestone Milestone => new MilestoneImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public DateTime? ProjectStartDate 
    {
        get {  return _dal.ProjectStartDate; } 
        set { if (value is not null && ProjectStartDate is null)
                _dal.ProjectStartDate = value; }
    }
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