namespace BlApi;

using BO;

/// <summary>
/// Interface for the operations on the Milestone entity.
/// </summary>
public interface IMilestone
{
    public void CreateProjectSchedule();
    public Milestone? Read(int id);
    public Milestone? Read(Func<Milestone, bool> filter);
    public Milestone? Update(int id);
}