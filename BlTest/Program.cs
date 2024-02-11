using BlApi;
using static DO.Exceptions;

namespace BlTest;

public class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// The main menu to choose an entity
    /// </summary>
    private static void Menu()
    {
        Console.WriteLine("Welcome to the Project Management Application!");
        int choice;
        try
        {
            do
            {
                Console.WriteLine(@$"You are now in the {s_bl.CheckProjectStatus()} stage of your project
Select which entity you want to test:
    0: Exit
    1: Engineer
    2: Task
        ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("Bye!");
                            break;
                        case 1:
                            SubMenu("Engineer", s_bl.Engineer);
                            break;
                        case 2:
                            SubMenu("Task", s_bl.Task);
                            break;
                        default:
                            Console.WriteLine("it must be a number between 0-2!");
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
    /// <param name="entity">engineer / task </param>
    /// <param name="s_blEntity">the interface field</param>
    private static void SubMenu<T>(string entity, T s_blEntity)
    {
        Console.WriteLine($"You chose the {entity} entity!");
        int choice;
        try
        {
            do
            {
                Console.WriteLine(@$"Select the opration you want to preform:
    0: Back to the main menu
    1: Create {entity}
    2: Read {entity}
    3: Read all {entity} list
    4: Update {entity}
    5: Delete {entity}
        ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("Back to the main menu.");
                            break;
                        case 1:
                            try
                            {
                                if (typeof(T) == typeof(IEngineer))
                                    CreateEngineer();

                                else if (typeof(T) == typeof(ITask))
                                    CreateTask();
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
                                    ReadEntity("engineer", s_bl!.Engineer.Read);

                                else if (typeof(T) == typeof(ITask))
                                    ReadEntity("task", s_bl!.Task.Read);
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
                                    ReadAllEntities(s_bl!.Engineer.ReadAll());

                                else if (typeof(T) == typeof(ITask))
                                    ReadAllEntities(s_bl!.Task.ReadAll());
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
                                    UpdateEngineer();

                                else if (typeof(T) == typeof(ITask))
                                    UpdateTask();
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
                                    DeleteEntity<IEngineer?>("Engineer", s_bl!.Engineer.Delete);

                                else if (typeof(T) == typeof(ITask))
                                    DeleteEntity<ITask?>("Task", s_bl!.Task.Delete);
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }
                            break;
                        default:
                            Console.WriteLine("it must be a number between 0-5!");
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
    /// create function for engineer entity
    /// </summary>
    /// <exception cref="FormatException">in case the parse didn't succeed</exception>
    static void CreateEngineer()
    {
        Console.WriteLine("Creating engineer");

        //getting the values from the user, throwing exception if the parse didn't succeed
        int id = GetValue<int>("Id", input => int.TryParse(input, out var parsedId) ? parsedId : throw new FormatException("invalid ID input"));
        string name = GetValue<string>("Name");
        string email = GetValue<string>("Email");
        BO.EngineerExperience level = GetValue<BO.EngineerExperience>("Level", input => Enum.TryParse(input, out BO.EngineerExperience parsedEnum) ? parsedEnum : throw new FormatException("input for level must be title (e.g. Beginner) or number between 0-4!"));
        double? cost = GetUpdatedValue("Cost", null, input => int.TryParse(input, out var parsedInt) ? parsedInt : (int?)null);

        BO.Engineer engineer = new()
        {
            Id = id, 
            Name = name, 
            Email = email, 
            Level = level, 
            Cost = cost,
            Task = null 
        };
        int newId = s_bl!.Engineer.Create(engineer);
        Console.WriteLine($"engineer created ID {newId}\n");
    }

    /// <summary>
    /// create function for task entity
    /// </summary>
    /// <exception cref="FormatException">in case the parse didn't succeed</exception>
    static void CreateTask()
    {
        Console.WriteLine("Creating task");

        //getting the values from the user, throwing exception if the parse didn't succeed, or null if nullable
        string alias = GetValue<string>("Alias");
        string description = GetValue<string>("Description");
        string deliverables = GetValue<string>("Deliverables");
        BO.EngineerExperience level = GetValue<BO.EngineerExperience>("DifficultyLevel", input => Enum.TryParse(input, out BO.EngineerExperience parsedLevel) ? parsedLevel : throw new FormatException("input for level must be title (e.g. Beginner) or number between 0-4!"));
        TimeSpan? effortTime = GetUpdatedValue("RequiredEffortTime", null, input => TimeSpan.TryParse(input, out var parsedTimeSpan) ? parsedTimeSpan : (TimeSpan?)null);
        DateTime? creation = GetUpdatedValue("CreatedAtDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        int? engineerId = GetUpdatedValue("EngineerId", null, input => int.TryParse(input, out var parsedInt) ? parsedInt : (int?)null);
        string? remarks = GetUpdatedValue<string?>("Remarks", null); //for nullable string
        DateTime? scheduledDate = GetUpdatedValue("ScheduledDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? startDate = GetUpdatedValue("StartDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? deadlineDate = GetUpdatedValue("DeadlineDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? completeDate = GetUpdatedValue("CompleteDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        
        BO.EngineerInTask? engineer = GetEngineerInTask(engineerId);
        ////need to add calc dependencies

        BO.Task task = new BO.Task
        {
            Id = 0,
            Alias = alias,
            Description = description,
            Deliverables = deliverables,
            DifficultyLevel = level,
            RequiredEffortTime = effortTime,
            CreatedAtDate = creation,
            Remarks = remarks,
            ScheduledDate = scheduledDate,
            StartDate = startDate,
            DeadlineDate = deadlineDate,
            CompleteDate = completeDate,
            Engineer = engineer
        };
        int newId = s_bl!.Task.Create(task);
        Console.WriteLine($"task created ID {newId}\n");
    }

    /// <summary>
    /// Updates an existing engineer based on user input.
    /// </summary>
    /// <exception cref="FormatException">in case the parse didn't succeed.</exception>
    /// <exception cref="DalDoesNotExistException">When the engineer with this ID is not found in the list.</exception>
    static void UpdateEngineer()
    {
        // Get the ID from the user
        Console.Write("Enter the Engineer ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var engineerId))
            throw new FormatException("Invalid input. Please enter a valid engineer ID.");
        BO.Engineer? existingEngineer = s_bl.Engineer.Read(engineerId);
        if (existingEngineer is not null)
            Console.WriteLine(existingEngineer);
        else
            throw new DalDoesNotExistException($"Engineer with ID={engineerId} does not exist.");

        // Ask the user for updated information
        Console.WriteLine("Enter updated information (press Enter to keep current value):");

        // Update engineer properties
        string name = GetUpdatedValue("Name", existingEngineer.Name);
        string email = GetUpdatedValue("Email", existingEngineer.Email);
        BO.EngineerExperience level = GetUpdatedValue("Level", existingEngineer.Level, input => Enum.TryParse(input, out BO.EngineerExperience parsedEnum) ? parsedEnum : throw new FormatException("invalid input for level"));
        double? cost = GetUpdatedValue("Cost", existingEngineer.Cost, input => double.TryParse(input, out var parsedDouble) ? parsedDouble : (double?)null);

        //setting the updated engineer
        BO.Engineer engineer = new()
        {
            Id = existingEngineer.Id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost,
            Task = null
        };
        s_bl!.Engineer.Update(engineer);
    }

    /// <summary>
    /// Updates an existing task based on user input.
    /// </summary>
    /// <exception cref="FormatException">in case the parse didn't succeed.</exception>
    /// <exception cref="DalDoesNotExistException">When the task with this ID is not found in the list.</exception>
    static void UpdateTask()
    {
        // Get the ID from the user
        Console.Write("Enter the Task ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var taskId))
            throw new FormatException("Invalid input. Please enter a valid Task ID.");
        BO.Task? existingTask = s_bl.Task.Read(taskId);
        if (existingTask is not null)
            Console.WriteLine(existingTask);
        else
            throw new DalDoesNotExistException($"Task with ID={taskId} does not exist.");

        // Ask the user for updated information
        Console.WriteLine("Enter updated information (press Enter to keep current value):");

        // Update task properties
        string alias = GetUpdatedValue("Alias", existingTask.Alias);
        string description = GetUpdatedValue("Description", existingTask.Description);
        string deliverables = GetUpdatedValue("Deliverables", existingTask.Deliverables);
        BO.EngineerExperience level = GetUpdatedValue("DifficultyLevel", existingTask.DifficultyLevel, input => Enum.TryParse(input, out BO.EngineerExperience parsedEnum) ? parsedEnum : existingTask.DifficultyLevel);
        TimeSpan? effortTime = GetUpdatedValue("RequiredEffortTime", existingTask.RequiredEffortTime, input => TimeSpan.TryParse(input, out var parsedTimeSpan) ? parsedTimeSpan : (TimeSpan?)null);
        DateTime? creation = GetUpdatedValue("CreatedAtDate", existingTask.CreatedAtDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        int? engineerId = GetUpdatedValue("EngineerId", existingTask.Engineer?.Id, input => int.TryParse(input, out var parsedInt) ? parsedInt : (int?)null);
        string? remarks = GetUpdatedValue("Remarks", existingTask.Remarks);
        DateTime? scheduledDate = GetUpdatedValue("ScheduledDate", existingTask.ScheduledDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? startDate = GetUpdatedValue("StartDate", existingTask.StartDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? deadlineDate = GetUpdatedValue("DeadlineDate", existingTask.DeadlineDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? completeDate = GetUpdatedValue("CompleteDate", existingTask.CompleteDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);

        BO.EngineerInTask? engineer = GetEngineerInTask(engineerId);
        BO.Task task = new BO.Task
        {
            Id = 0,
            Alias = alias,
            Description = description,
            Deliverables = deliverables,
            DifficultyLevel = level,
            RequiredEffortTime = effortTime,
            CreatedAtDate = creation,
            Remarks = remarks,
            ScheduledDate = scheduledDate,
            StartDate = startDate,
            DeadlineDate = deadlineDate,
            CompleteDate = completeDate,
            Engineer = engineer
        };
        s_bl!.Task.Update(task);
    }

    static BO.EngineerInTask? GetEngineerInTask(int? engineerId)
    {
        if (engineerId is not null)
        {
            BO.EngineerInTask engineer = new BO.EngineerInTask
            {
                Id = (int)engineerId,
                Name = null
            };
            return engineer;
        }
        return null;
    }

    /// <summary>
    /// a generic function to read entity (even if it's inactive in engineer / task)
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entity">engineer/task/dependency</param>
    /// <param name="read">the read function from the entity's interface</param>
    /// <exception cref="FormatException">in case the parse didn't succeed</exception>
    /// <exception cref="DalDoesNotExistException">in case the entity with this ID not exist</exception>
    static void ReadEntity<T>(string entity, Func<int, T?> read)
    {
        Console.WriteLine($"You chose read. Enter ID for {entity}");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new FormatException("ID must be a number!");
        T? ent = read(id);

        if (ent is not null)
            Console.WriteLine(ent);
        else
            throw new DalDoesNotExistException($"{entity} with ID={id} not found");
    }

    /// <summary>
    /// a generic function to read all the list of entities
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entities">the readAll function from the entity's interface</param>
    static void ReadAllEntities<T>(IEnumerable<T?> entities)
    {
        entities.ToList().ForEach(item => Console.WriteLine(item));
    }

    /// <summary>
    /// a generic deletion function to delete entity from the list
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entity">engineer/task/dependency</param>
    /// <param name="delete">the delete function from the entity's interface</param>
    /// <exception cref="FormatException">in case the parse didn't succeed</exception>
    static void DeleteEntity<T>(string entity, Action<int> delete)
    {
        Console.WriteLine($"enter the {entity} ID");
        if (!int.TryParse(Console.ReadLine(), out var id))
            throw new FormatException("ID must be a number!");
        delete(id);
    }

    /// <summary>
    /// Helper method to get an new value from the user.
    /// </summary>
    /// <typeparam name="T">Type of the property being created.</typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="parser">Parser function for the property type.</param>
    /// <returns>The new value</returns>
    /// <exception cref="FormatException">if the user provided no input or bad input.</exception>
    private static T GetValue<T>(string propertyName, Func<string, T>? parser = null)
    {
        Console.Write($"{propertyName}: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            throw new FormatException($"You must enter {propertyName}!");

        return ConvertType(input, parser);
    }

    /// <summary>
    /// Helper function to parse and convert input to a specific type
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="input">the input form the user</param>
    /// <param name="parser">the parse method or null</param>
    /// <returns></returns>
    private static T ConvertType<T>(string input, Func<string, T>? parser = null)
    {
        if (parser != null)
            return parser(input);

        Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        return (T)Convert.ChangeType(input, targetType);
    }

    /// <summary>
    /// Helper method to get an updated or nullable (optional) value from the user.
    /// </summary>
    /// <typeparam name="T">Type of the property being updated.</typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="currentValue">Current value of the property.</param>
    /// <param name="parser">Parser function for the property type.</param>
    /// <returns>The updated value or the current value if the user provided no input.</returns>
    private static T GetUpdatedValue<T>(string propertyName, T currentValue, Func<string, T>? parser = null)
    {
        Console.Write($"{propertyName} ({currentValue}): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return currentValue;

        try
        {
            return ConvertType(input, parser);
        }
        catch (Exception)
        {
            Console.WriteLine($"Invalid input for {propertyName}. Keeping current value.");
            return currentValue;
        }
    }

    /// <summary>
    /// The main of the program. Only operating the initialization and the menu.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y" || ans == "y")
            DalTest.Initialization.Do();

        Menu();
    }
}
