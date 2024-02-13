using DalApi;

namespace BlApi;

/// <summary>
/// Interface for the Bl
/// </summary>
public interface IBl
{
    IMilestone Milestone { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    public DateTime? ProjectStartDate { get; set; }
    public BO.ProjectStatus CheckProjectStatus();
}