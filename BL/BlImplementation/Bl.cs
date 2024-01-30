using BlApi;

namespace BlImplementation;
sealed public class Bl : IBl
{
    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
}