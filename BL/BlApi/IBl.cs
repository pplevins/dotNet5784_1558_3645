namespace BlApi;

/// <summary>
/// Interface for the Bl
/// </summary>
public interface IBl
{
    public void InitializeDB();
    public void ResetDB();
    IMilestone Milestone { get; }
    IEngineer Engineer { get; }
    IUser User { get; }
    ITask Task { get; }
    public DateTime? ProjectStartDate { get; set; }
    public BO.ProjectStatus CheckProjectStatus();
}