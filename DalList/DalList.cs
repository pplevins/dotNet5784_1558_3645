using DalApi;
namespace Dal;

sealed internal class DalList : IDal
{
    private static readonly Lazy<IDal> lazyInstance = new Lazy<IDal>(() => new DalList(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static IDal Instance => lazyInstance.Value;
    private DalList() { }

    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IUser User => new UserImplementation();
    public ITask Task => new TaskImplementation();
    public DateTime? ProjectStartDate
    {
        get { return Dal.DataSource.Config.ProjectStartDate; }
        set { Dal.DataSource.Config.ProjectStartDate = value; }
    }
}
