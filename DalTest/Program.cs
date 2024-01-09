using Dal;
using DalApi;
using DO;
using System.Xml.Linq;

namespace DalTest;

/// <summary>
/// The main program to test the dal
/// </summary>
public class Program
{
    //initializing the fields for the interfaces 
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); 
    private static ITask? s_dalTask = new TaskImplementation(); 
    private static IDependency? s_dalDependency = new DependencyImplementation();

    /// <summary>
    /// The main menu to choose an entity
    /// </summary>
    /// <exception cref="Exception">in case the choice was not a number</exception>
    private static void Menu()
    {
        Console.WriteLine(@"Welcome to the Project Management Application!
Select which entity you want to test:
    0: Exit
    1: Engineer
    2: Task
    3: Dependency
        ");
        
        int choice;
        try
        {
            do
            {
                Console.WriteLine("\nselect entity:");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("Bye!");
                            break;
                        case 1:
                            SubMenu("Engineer", s_dalEngineer);
                            break;
                        case 2:
                            SubMenu("Task", s_dalTask);
                            break;
                        case 3:
                            SubMenu("Dependency", s_dalDependency);
                            break;
                        default:
                            Console.WriteLine("it must be a number between 0-3!");
                            break;
                    }
                }
                else { Console.WriteLine("The input must be a number!"); }
            } while (choice != 0);
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
    }

    /// <summary>
    /// Genreric sub menu for each entity. choosing the operation to test.
    /// </summary>
    /// <typeparam name="T">The generic type for the function</typeparam>
    /// <param name="entity">engineer / task / dependency</param>
    /// <param name="s_dalEntity">the interface field</param>
    /// <exception cref="Exception">in case the choice is not a number</exception>
    private static void SubMenu<T>(string entity, T s_dalEntity)
    {
        Console.WriteLine(@$"You chose the {entity} entity!
Select the opration you want to preform:
    0: Back to the main menu
    1: Create {entity}
    2: Read {entity}
    3: Read all {entity} list
    4: Update {entity}
    5: Delete {entity}
    6: Reset all {entity} list
        ");
        int choice;
        try
        {
            do
            {
                Console.WriteLine("\nselect your operation:");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("Back to the main menu. choose your entity:");
                            break;
                        case 1:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    CreateEngineer();

                                else if (typeof(T) == typeof(ITask))
                                    CreateTask();

                                else if (typeof(T) == typeof(IDependency))
                                    CreateDependency();
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        case 2:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    ReadEntity("engineer", s_dalEngineer!.Read);

                                else if (typeof(T) == typeof(ITask))
                                    ReadEntity("task", s_dalTask!.Read);

                                else if (typeof(T) == typeof(IDependency))
                                    ReadEntity("dependency", s_dalDependency!.Read);
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        case 3:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    ReadAllEntities(s_dalEngineer!.ReadAll);
                                else if (typeof(T) == typeof(ITask))
                                    ReadAllEntities(s_dalTask!.ReadAll);
                                else if (typeof(T) == typeof(IDependency))
                                    ReadAllEntities(s_dalDependency!.ReadAll);
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        case 4:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    UpdateEngineer(s_dalEngineer!.Read);
                                else if (typeof(T) == typeof(ITask))
                                    UpdateTask(s_dalTask!.Read);
                                else if (typeof(T) == typeof(IDependency))
                                    UpdateDependency(s_dalDependency!.Read);
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        case 5:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    DeleteEntity<IEngineer?>("Engineer", s_dalEngineer!.Delete);
                                else if (typeof(T) == typeof(ITask))
                                    DeleteEntity<ITask?>("Task", s_dalTask!.Delete);
                                else if (typeof(T) == typeof(IDependency))
                                    DeleteEntity<IDependency?>("Dependency", s_dalDependency!.Delete);
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        case 6:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    s_dalEngineer!.Reset();
                                else if (typeof(T) == typeof(ITask))
                                    s_dalTask!.Reset();
                                else if (typeof(T) == typeof(IDependency))
                                    s_dalDependency!.Reset();
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        default:
                            Console.WriteLine("it must be a number between 0-6!");
                            break;
                    }
                }
                else { Console.WriteLine("The input must be a number!"); }
            } while (choice != 0);
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
    }

    /// <summary>
    /// create function for task entity
    /// </summary>
    /// <exception cref="Exception"></exception>
    static void CreateEngineer()
    {
        Console.WriteLine("enter ID");
        if(!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");

        Console.WriteLine("enter name");
        string name = Console.ReadLine() ?? throw new Exception("it can't be null!");
        if (String.IsNullOrWhiteSpace(name))
            throw new Exception("you must enter name!");

        Console.WriteLine("enter Email");
        string email = Console.ReadLine() ?? throw new Exception("it can't be null!");
        if (String.IsNullOrWhiteSpace(email))
            throw new Exception("you must enter email!");

        Console.WriteLine("enter experience level in name (e.g. Begginer) or number between 0-4");
        if (!Enum.TryParse(Console.ReadLine(), out EngineerExperience level))
            throw new Exception("it must be title (e.g. Beginner) or number between 0-4!");

        Console.WriteLine("enter cost for hour");
        double.TryParse(Console.ReadLine(), out var cost);

        Engineer engineer = new(id, name, email, level, cost);
        int newId = s_dalEngineer!.Create(engineer);
        Console.WriteLine($"engineer created ID {newId}\n");
    }

    /// <summary>
    /// create function for engineer entity
    /// </summary>
    /// <exception cref="Exception"></exception>
    static void CreateTask()
    {
        Console.WriteLine("enter alias");
        string alias = Console.ReadLine() ?? throw new Exception("it can't be null!");
        if (String.IsNullOrWhiteSpace(alias))
            throw new Exception("you must enter alias!");

        Console.WriteLine("enter description");
        string description = Console.ReadLine() ?? throw new Exception("it can't be null!");
        if (String.IsNullOrWhiteSpace(description))
            throw new Exception("you must enter description!");

        Console.WriteLine("enter Deliverables");
        string deliverables = Console.ReadLine() ?? throw new Exception("it can't be null!");
        if (String.IsNullOrWhiteSpace(deliverables))
            throw new Exception("you must enter deliverables!");

        Console.WriteLine("enter Difficulty Level in name (e.g. Begginer) or number between 0-4");
        if (!Enum.TryParse(Console.ReadLine(), out EngineerExperience level))
            throw new Exception("it must be title (e.g. Beginner) or number between 0-4!");

        Console.WriteLine("enter IsMilestone:");
        if (!bool.TryParse(Console.ReadLine(), out var isMilestone))
            isMilestone = false;

        Console.WriteLine("enter Required Effort Time in TimeSpan format:");
        string? tring = Console.ReadLine();
        TimeSpan? effortTime = ParseNullableTimeSpan(tring);

        Console.WriteLine("enter Creation Date:");
        string? crat = Console.ReadLine();
        DateTime? creation = ParseNullableDateTime(crat);

        Console.WriteLine("enter engineer id:");
        string? eId = Console.ReadLine();
        int? engineerId = ParseNullableInt(eId);

        Console.WriteLine("enter remarks:");
        string? remarks = Console.ReadLine();

        Console.WriteLine("enter Scheduled Date:");
        string? schDate = Console.ReadLine();
        DateTime? scheduledDate = ParseNullableDateTime(schDate);

        Console.WriteLine("enter Start Date:");
        string? start = Console.ReadLine();
        DateTime? startDate = ParseNullableDateTime(start);

        Console.WriteLine("enter Deadline Date:");
        string? deadline = Console.ReadLine();
        DateTime? deadlineDate = ParseNullableDateTime(deadline);

        Console.WriteLine("enter Complete Date:");
        string? complete = Console.ReadLine();
        DateTime? completeDate = ParseNullableDateTime(complete);

        DO.Task task = new(0, alias, description, isMilestone, deliverables, level, effortTime, creation, engineerId, remarks, scheduledDate, startDate, deadlineDate, completeDate);
        int newId = s_dalTask!.Create(task);
        Console.WriteLine($"task created ID {newId}\n");
    }

    /// <summary>
    /// create function for dependency entity
    /// </summary>
    /// <exception cref="Exception"></exception>
    static void CreateDependency()
    {
        Console.WriteLine("enter Dependent Task ID");
        if (!int.TryParse(Console.ReadLine(), out var dependent))
            throw new Exception("ID must be a number!");

        Console.WriteLine("enter Previous Task ID");
        if (!int.TryParse(Console.ReadLine(), out var previous))
            throw new Exception("ID must be a number!");

        Dependency dependency = new(0, dependent, previous);
        int newId = s_dalDependency!.Create(dependency);
        Console.WriteLine($"Dependency created ID {newId}");
    }


    /// <summary>
    /// a generic function to read entity
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entity">engineer/task/dependency</param>
    /// <param name="read">the read function from the entity's interface</param>
    /// <exception cref="Exception"></exception>
    static void ReadEntity<T>(string entity, Func<int, T?> read)
    {
        Console.WriteLine($"You chose read. Enter ID for {entity}");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");
        T? ent = read(id);
        if (ent is not null)
            Console.WriteLine(ent);
        else
            throw new Exception($"{entity} not found");
    }

    /// <summary>
    /// a generic function to read all the list of entities
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="readAll">the readAll function from the entity's interface</param>
    static void ReadAllEntities<T>(Func<List<T?>> readAll)
    {
        List<T?> entities = readAll();
        foreach (var item in entities)
        {
            Console.WriteLine(item);
        }
    }

    /// <summary>
    /// a generic deletion function to delete entity from the list
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entity">engineer/task/dependency</param>
    /// <param name="delete">the delete function from the entity's interface</param>
    /// <exception cref="Exception"></exception>
    static void DeleteEntity<T>(string entity, Action<int> delete)
    {
        Console.WriteLine($"enter the {entity} ID");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");
        delete(id);
    }


    /// <summary>
    /// an update function for the engineer entity
    /// </summary>
    /// <param name="read">the read function from the engineer's interface</param>
    /// <exception cref="Exception"></exception>
    static void UpdateEngineer(Func<int, Engineer?> read)
    {
        Console.WriteLine("You chose update. Enter ID for engineer");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");
        Engineer? ent = read(id);
        if (ent is not null)
            Console.WriteLine(ent);
        else
            throw new Exception("engineer not found");

        //asking each property from the user.
        string name;
        string email;

        Console.WriteLine("enter name:");
        name = Console.ReadLine() ?? ent.Name;
        if (String.IsNullOrWhiteSpace(name))
            name = ent.Name;

        Console.WriteLine("enter email:");
        email = Console.ReadLine() ?? ent.Email;
        if (String.IsNullOrWhiteSpace(email))
            email = ent.Email;

        Console.WriteLine("enter level:");
        if (!Enum.TryParse(Console.ReadLine(), out EngineerExperience level))
            level = ent.Level;

        Console.WriteLine("enter cost:");
        double.TryParse(Console.ReadLine(), out var cost);

        //setting the updated engineer
        s_dalEngineer!.Update(new(ent.Id, name, email, level, cost));
    }

    /// <summary>
    /// an update function for the task entity
    /// </summary>
    /// <param name="read">the read function from the task's interface</param>
    /// <exception cref="Exception"></exception>
    static void UpdateTask(Func<int, DO.Task?> read)
    {
        Console.WriteLine("You chose update. Enter ID for task");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");
        DO.Task? ent = read(id);
        if (ent is not null)
            Console.WriteLine(ent);
        else
            throw new Exception("engineer not found");

        //asking each property from the user
        string alias;
        string description;
        string deliverables;

        Console.WriteLine("enter alias:");
        alias = Console.ReadLine() ?? ent.Alias;
        if (String.IsNullOrWhiteSpace(alias))
            alias = ent.Alias;

        Console.WriteLine("enter description:");
        description = Console.ReadLine() ?? ent.Description;
        if (String.IsNullOrWhiteSpace(description))
            description = ent.Description;

        Console.WriteLine("enter IsMilestone:");
        if (!bool.TryParse(Console.ReadLine(), out var isMilestone))
            isMilestone = ent.IsMilestone;

        Console.WriteLine("enter Deliverables:");
        deliverables = Console.ReadLine() ?? ent.Deliverables;
        if (String.IsNullOrWhiteSpace(deliverables))
            deliverables = ent.Deliverables;

        Console.WriteLine("enter Difficulty Level:");
        if (!Enum.TryParse(Console.ReadLine(), out EngineerExperience level))
            level = ent.DifficultyLevel;

        Console.WriteLine("enter Required Effort Time in TimeSpan format:");
        string? tring = Console.ReadLine();
        TimeSpan? effortTime = ParseNullableTimeSpan(tring) ?? ent.RequiredEffortTime;

        Console.WriteLine("enter Creation Date:");
        string? crat = Console.ReadLine();
        DateTime? creation = ParseNullableDateTime(crat) ?? ent.CreatedAtDate;
        
        Console.WriteLine("enter engineer id:");
        string? eId = Console.ReadLine();
        int? engineerId = ParseNullableInt(eId) ?? ent.EngineerId;

        Console.WriteLine("enter remarks:");
        string? remarks = Console.ReadLine() ?? ent.Remarks;
        if (String.IsNullOrWhiteSpace(remarks))
            remarks = ent.Remarks;

        Console.WriteLine("enter Scheduled Date:");
        string? schDate = Console.ReadLine();
        DateTime? scheduledDate = ParseNullableDateTime(schDate) ?? ent.ScheduledDate;

        Console.WriteLine("enter Start Date:");
        string? start = Console.ReadLine();
        DateTime? startDate = ParseNullableDateTime(start) ?? ent.StartDate;

        Console.WriteLine("enter Deadline Date:");
        string? deadline = Console.ReadLine();
        DateTime? deadlineDate = ParseNullableDateTime(deadline) ?? ent.DeadlineDate;

        Console.WriteLine("enter Complete Date:");
        string? complete = Console.ReadLine();
        DateTime? completeDate = ParseNullableDateTime(complete) ?? ent.CompleteDate;

        s_dalTask!.Update(new(ent.Id, alias, description, isMilestone, deliverables, level, effortTime, creation, engineerId, remarks, scheduledDate, startDate, deadlineDate, completeDate));
    }

    /// <summary>
    /// an helper function to convert TimeSpan to nullabe Timespan
    /// </summary>
    /// <param name="input">input string from the user</param>
    /// <returns>or a TimeSpan? or null</returns>
    static TimeSpan? ParseNullableTimeSpan(string? input)
    {
        if (TimeSpan.TryParse(input, out TimeSpan result))
            return result;
        else
            return null;
    }

    /// <summary>
    /// an helper function to convert DateTime to nullabe DateTime
    /// </summary>
    /// <param name="input">input string from the user</param>
    /// <returns>or DateTime? or null</returns>
    static DateTime? ParseNullableDateTime(string? input)
    {
        if (DateTime.TryParse(input, out DateTime result))
            return result;
        else
            return null;
    }

    /// <summary>
    /// an helper function to convert int to nullabe int
    /// </summary>
    /// <param name="input">input string from the user</param>
    /// <returns>or int? or null</returns>
    static int? ParseNullableInt(string? input)
    {
        if (int.TryParse(input, out int result))
            return result;
        else
            return null;
    }

    /// <summary>
    /// an update function for the dependency entity
    /// </summary>
    /// <param name="read">the read function from the dependency's interface</param>
    /// <exception cref="Exception"></exception>
    static void UpdateDependency(Func<int, DO.Dependency?> read)
    {
        Console.WriteLine("You chose update. Enter ID for dependency");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");
        DO.Dependency? ent = read(id);
        if (ent is not null)
            Console.WriteLine(ent);
        else
            throw new Exception("dependency not found");

        Console.WriteLine("enter dependent task:");
        if (!int.TryParse(Console.ReadLine(), out int dependent))
            dependent = ent.DependentTask;

        Console.WriteLine("enter previous task:");
        if (!int.TryParse(Console.ReadLine(), out int previous))
            previous = ent.PreviousTask;

        s_dalDependency!.Update(new(ent.Id, dependent, previous));
    }

    /// <summary>
    /// The main of the program. Only operating the initialization and the menu.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
        Menu();
    }
}
