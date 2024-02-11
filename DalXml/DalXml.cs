using DalApi;
namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    private static readonly Lazy<IDal> lazyInstance = new Lazy<IDal>(() => new DalXml(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static IDal Instance => lazyInstance.Value;
    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public DateTime? ProjectStartDate 
    { 
        get { return Config.ProjectStartDate; }
        set { Config.ProjectStartDate = value; }
    }
}
