using BlApi;

namespace BlImplementation;
sealed public class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IMilestone Milestone => new MilestoneImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public DateTime? ProjectStartDate { get; set; } = null;
    public BO.ProjectStatus CheckProjectStatus()
    {
        if (ProjectStartDate == null)
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