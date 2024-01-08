using Dal;
using DalApi;
using DO;

namespace DalTest;

public class Program
{
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); 
    private static ITask? s_dalTask = new TaskImplementation(); 
    private static IDependency? s_dalDependency = new DependencyImplementation();

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
        do
        {
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                { 
                    case 0:
                        
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
            else { throw new Exception("The input must be a number!"); }
        } while (choice != 0);
    }

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
        do
        {
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 0:
                        
                        break;
                    case 1:
                        if (typeof(T) == typeof(IEngineer))
                            CreateEngineer();

                        else if (typeof(T) == typeof(ITask))
                            CreateTask();
                        
                        else if (typeof(T) == typeof(IDependency))
                            CreateDependency();
                        break;
                    case 2:
                        if (typeof(T) == typeof(IEngineer))
                            ReadEntity("engineer", s_dalEngineer!.Read);

                        else if (typeof(T) == typeof(ITask))
                            ReadEntity("task", s_dalTask!.Read);

                        else if (typeof(T) == typeof(IDependency))
                            ReadEntity("dependency", s_dalDependency!.Read);
                        break;
                    //case 3:
                    //    if (typeof(T) == typeof(IEngineer))
                    //        ReadAllEngineer();
                    //    else if (typeof(T) == typeof(ITask))
                    //        ReadAllTask();
                    //    else if (typeof(T) == typeof(IEngineer))
                    //        ReadAllDependency();
                    //    break;
                    //case 4:
                    //    if (typeof(T) == typeof(IEngineer))
                    //        UpdateEngineer();
                    //    else if (typeof(T) == typeof(ITask))
                    //        UpdateTask();
                    //    else if (typeof(T) == typeof(IEngineer))
                    //        UpdateDependency();
                    //    break;
                    //case 5:
                    //    if (typeof(T) == typeof(IEngineer))
                    //        DeleteEngineer();
                    //    else if (typeof(T) == typeof(ITask))
                    //        DeleteTask();
                    //    else if (typeof(T) == typeof(IEngineer))
                    //        DeleteDependency();
                    //    break;
                    //case 6:
                    //    if (typeof(T) == typeof(IEngineer))
                    //        ResetEngineer();
                    //    else if (typeof(T) == typeof(ITask))
                    //        ResetTask();
                    //    else if (typeof(T) == typeof(IEngineer))
                    //        ResetDependency();
                    //    break;
                    default:
                        Console.WriteLine("it must be a number between 0-6!");
                        break;
                }
            }
            else { throw new Exception("The input must be a number!"); }
        } while (choice != 0);
    }

    static void CreateEngineer()
    {
        Console.WriteLine("enter ID");
        if(!int.TryParse(Console.ReadLine(), out var id))
            throw new Exception("ID must be a number!");

        Console.WriteLine("enter name");
        string name = Console.ReadLine() ?? throw new Exception("it can't be null!");

        Console.WriteLine("enter Email");
        string email = Console.ReadLine() ?? throw new Exception("it can't be null!");

        Console.WriteLine("enter experience level in name (e.g. Begginer) or number between 0-4");
        if (!Enum.TryParse(Console.ReadLine(), out EngineerExperience level))
            throw new Exception("it must be title (e.g. Beginner) or number between 0-4!");

        Engineer engineer = new(id, name, email, level);
        int newId = s_dalEngineer!.Create(engineer);
        Console.WriteLine($"engineer created ID {newId}\n");
    }

    static void CreateTask()
    {
        Console.WriteLine("enter alias");
        string alias = Console.ReadLine() ?? throw new Exception("it can't be null!");

        Console.WriteLine("enter description");
        string description = Console.ReadLine() ?? throw new Exception("it can't be null!");

        Console.WriteLine("enter Deliverables");
        string deliverables = Console.ReadLine() ?? throw new Exception("it can't be null!");

        Console.WriteLine("enter Difficulty Level in name (e.g. Begginer) or number between 0-4");
        if (!Enum.TryParse(Console.ReadLine(), out EngineerExperience level))
            throw new Exception("it must be title (e.g. Beginner) or number between 0-4!");

        DO.Task task = new(0, alias, description, false, deliverables, level);
        int newId = s_dalTask!.Create(task);
        Console.WriteLine($"task created ID {newId}\n");
    }

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

    static void ReadAllEntities<T>(Func<List<T?>> readAll)
    {

    }

    static void Main(string[] args)
    {
        Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
        Menu();
    }
}
