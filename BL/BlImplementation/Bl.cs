using BlApi;

namespace BlImplementation;
internal class Bl : IBl
{
    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB()
    {
        Engineer.Reset();
        User.Reset();
        Task.Reset();
        ProjectStartDate = null;
    }

    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IMilestone Milestone => new MilestoneImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IUser User => new UserImplementation();
    public ITask Task => new TaskImplementation(this);

    /// <summary>
    /// The set/get property of the project start date
    /// </summary>
    public DateTime? ProjectStartDate
    {
        get { return _dal.ProjectStartDate; }
        set
        {
            if (ProjectStartDate is not null)
                throw new InvalidOperationException("There's already date for the project. You can't reset.");
            if (value < Clock)
                throw new InvalidOperationException("You can't set a past date.");
            if (value is not null)
                _dal.ProjectStartDate = value;
        }
    }

    /// <summary>
    /// Checking the project status according to the stages
    /// </summary>
    /// <returns>Enum of the project status</returns>
    public BO.ProjectStatus CheckProjectStatus()
    {
        if (ProjectStartDate is null)
            return BO.ProjectStatus.Planing;
        else
        {
            if (_dal.Task.ReadAll().All(task => task?.ScheduledDate is not null) && IsProjectStartDateBeforeSystemClock())
                return BO.ProjectStatus.InProgress;
            else
                return BO.ProjectStatus.MiddlePlaning;
        }
    }
    /// <summary>
    /// Checks if the project start date is before the current system clock time.
    /// </summary>
    /// <returns>True if the project start date is before the system clock time, otherwise false.</returns>
    private bool IsProjectStartDateBeforeSystemClock()
    {
        return (ProjectStartDate.HasValue && DateTime.Compare(ProjectStartDate.Value, Clock) <= 0);
    }

    public void AdvanceYear(int years)
    {
        Clock = Clock.AddYears(years);
    }

    public void AdvanceMonth(int months)
    {
        Clock = Clock.AddMonths(months);
    }

    public void AdvanceDay(int days)
    {
        Clock = Clock.AddDays(days);
    }

    public void AdvanceHour(int hours)
    {
        Clock = Clock.AddHours(hours);
    }

    public void ResetTime()
    {
        Clock = DateTime.Now;
    }
}