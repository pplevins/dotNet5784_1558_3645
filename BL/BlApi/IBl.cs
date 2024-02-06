using DalApi;

namespace BlApi;

public interface IBl
{
    IMilestone Milestone { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    public DateTime? ProjectStartDate { get; set; }
    public BO.ProjectStatus CheckProjectStatus();
}