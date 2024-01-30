namespace BlApi;

public interface IBl
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
}