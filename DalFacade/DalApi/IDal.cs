namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    IUser User { get; }
    ITask Task { get; }
    public DateTime? ProjectStartDate { get; set; }
}