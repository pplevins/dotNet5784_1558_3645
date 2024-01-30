using DalApi;
using DO;
using static DO.Exceptions;

namespace DalTest;

/// <summary>
/// The main program to test the dal
/// </summary>
public class Program
{
    //initializing the fields for the interfaces
    //static readonly IDal s_dal = new DalList(); //stage 2
    //static readonly IDal s_dal = new DalXml(); //stage 3
    static readonly IDal s_dal = Factory.Get; //stage 4

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
                Console.WriteLine(@"Select which entity you want to test:
    0: Exit
    1: Engineer
    2: Task
    3: Dependency
        ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("Bye!");
                            break;
                        case 1:
                            SubMenu("Engineer", s_dal.Engineer);
                            break;
                        case 2:
                            SubMenu("Task", s_dal.Task);
                            break;
                        case 3:
                            SubMenu("Dependency", s_dal.Dependency);
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
    private static void SubMenu<T>(string entity, T s_dalEntity)
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
    6: Reset all {entity} list
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
                                    ReadEntity("engineer", s_dal!.Engineer.Read);

                                else if (typeof(T) == typeof(ITask))
                                    ReadEntity("task", s_dal!.Task.Read);

                                else if (typeof(T) == typeof(IDependency))
                                    ReadEntity("dependency", s_dal!.Dependency.Read);
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
                                    ReadAllEntities(s_dal!.Engineer.ReadAll());

                                else if (typeof(T) == typeof(ITask))
                                    ReadAllEntities(s_dal!.Task.ReadAll());

                                else if (typeof(T) == typeof(IDependency))
                                    ReadAllEntities(s_dal!.Dependency.ReadAll());
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

                                else if (typeof(T) == typeof(IDependency))
                                    UpdateDependency();
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
                                    DeleteEntity<IEngineer?>("Engineer", s_dal!.Engineer.Delete);

                                else if (typeof(T) == typeof(ITask))
                                    DeleteEntity<ITask?>("Task", s_dal!.Task.Delete);

                                else if (typeof(T) == typeof(IDependency))
                                    DeleteEntity<IDependency?>("Dependency", s_dal!.Dependency.Delete);
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
                                    s_dal!.Engineer.Reset();

                                else if (typeof(T) == typeof(ITask))
                                    s_dal!.Task.Reset();

                                else if (typeof(T) == typeof(IDependency))
                                    s_dal!.Dependency.Reset();
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
        EngineerExperience level = GetValue<EngineerExperience>("Level", input => Enum.TryParse(input, out DO.EngineerExperience parsedEnum) ? parsedEnum : throw new FormatException("input for level must be title (e.g. Beginner) or number between 0-4!"));
        double? cost = GetUpdatedValue("Cost", null, input => int.TryParse(input, out var parsedInt) ? parsedInt : (int?)null);

        Engineer engineer = new(id, name, email, level, true, cost);
        int newId = s_dal!.Engineer.Create(engineer);
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
        EngineerExperience level = GetValue<EngineerExperience>("DifficultyLevel", input => Enum.TryParse(input, out DO.EngineerExperience parsedLevel) ? parsedLevel : throw new FormatException("input for level must be title (e.g. Beginner) or number between 0-4!"));
        bool isMilestone = GetValue("isMilestone", input => bool.TryParse(input, out bool parsedInput) ? parsedInput : false);
        TimeSpan? effortTime = GetUpdatedValue("RequiredEffortTime", null, input => TimeSpan.TryParse(input, out var parsedTimeSpan) ? parsedTimeSpan : (TimeSpan?)null);
        DateTime? creation = GetUpdatedValue("CreatedAtDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        int? engineerId = GetUpdatedValue("EngineerId", null, input => int.TryParse(input, out var parsedInt) ? parsedInt : (int?)null);
        string? remarks = GetUpdatedValue<string?>("Remarks", null); //for nullable string
        DateTime? scheduledDate = GetUpdatedValue("ScheduledDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? startDate = GetUpdatedValue("StartDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? deadlineDate = GetUpdatedValue("DeadlineDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? completeDate = GetUpdatedValue("CompleteDate", null, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);

        DO.Task task = new(0, alias, description, isMilestone, deliverables, level, effortTime, creation, engineerId, remarks, scheduledDate, startDate, deadlineDate, completeDate);
        int newId = s_dal!.Task.Create(task);
        Console.WriteLine($"task created ID {newId}\n");
    }

    /// <summary>
    /// create function for dependency entity
    /// </summary>
    /// <exception cref="FormatException">in case the parse didn't succeed</exception>
    static void CreateDependency()
    {
        Console.WriteLine("Creating dependency");

        //getting the values from the user, throwing exception if the parse didn't succeed, or null if nullable
        int dependent = GetValue("DependentTask", input => int.TryParse(input, out var parsedInt) ? parsedInt : throw new FormatException("invalid input for dependentTask ID"));
        int previous = GetValue("PreviousTask", input => int.TryParse(input, out var parsedInt) ? parsedInt : throw new FormatException("invalid input for previousTask ID"));

        Dependency dependency = new(0, dependent, previous);
        int newId = s_dal!.Dependency.Create(dependency);
        Console.WriteLine($"Dependency created ID {newId}");
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

        // Check if the entity has an "isActive" property, and print note about it
        CheckActive(entity, ent);

        if (ent is not null)
            Console.WriteLine(ent);
        else
            throw new DalDoesNotExistException($"{entity} with ID={id} not found");
    }

    /// <summary>
    /// Helper function to check if the read entity is inactive, and prints note to the user about it
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entity">engineer/task/dependency (not relevant)</param>
    /// <param name="ent">the entity itself</param>
    static void CheckActive<T>(string entity, T? ent)
    {
        var isActiveProperty = typeof(T).GetProperty("IsActive");

        if (isActiveProperty is not null && isActiveProperty.PropertyType == typeof(bool))
        {
            // If "isActive" property exists and is of type bool, check its value
            bool isActive = (bool)isActiveProperty.GetValue(ent);

            if (!isActive)
                Console.WriteLine($"NOTE: This is an inactive {entity}!");
        }
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
        DO.Engineer? existingEngineer = s_dal.Engineer.Read(engineerId);
        if (existingEngineer is not null)
            Console.WriteLine(existingEngineer);
        else
            throw new DalDoesNotExistException($"Engineer with ID={engineerId} does not exist.");

        // Ask the user for updated information
        Console.WriteLine("Enter updated information (press Enter to keep current value):");

        // Update engineer properties
        string name = GetUpdatedValue("Name", existingEngineer.Name);
        string email = GetUpdatedValue("Email", existingEngineer.Email);
        EngineerExperience level = GetUpdatedValue("Level", existingEngineer.Level, input => Enum.TryParse(input, out DO.EngineerExperience parsedEnum) ? parsedEnum : throw new FormatException("invalid input for level"));
        double? cost = GetUpdatedValue("Cost", existingEngineer.Cost, input => double.TryParse(input, out var parsedDouble) ? parsedDouble : (double?)null);

        //setting the updated engineer
        s_dal!.Engineer.Update(new(existingEngineer.Id, name, email, level, true, cost));
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
        DO.Task? existingTask = s_dal.Task.Read(taskId);
        if (existingTask is not null)
            Console.WriteLine(existingTask);
        else
            throw new DalDoesNotExistException($"Task with ID={taskId} does not exist.");

        // Ask the user for updated information
        Console.WriteLine("Enter updated information (press Enter to keep current value):");

        // Update task properties
        string alias = GetUpdatedValue("Alias", existingTask.Alias);
        string description = GetUpdatedValue("Description", existingTask.Description);
        bool isMilestone = GetUpdatedValue("IsMilestone", existingTask.IsMilestone);
        string deliverables = GetUpdatedValue("Deliverables", existingTask.Deliverables);
        EngineerExperience level = GetUpdatedValue("DifficultyLevel", existingTask.DifficultyLevel, input => Enum.TryParse(input, out DO.EngineerExperience parsedEnum) ? parsedEnum : existingTask.DifficultyLevel);
        TimeSpan? effortTime = GetUpdatedValue("RequiredEffortTime", existingTask.RequiredEffortTime, input => TimeSpan.TryParse(input, out var parsedTimeSpan) ? parsedTimeSpan : (TimeSpan?)null);
        DateTime? creation = GetUpdatedValue("CreatedAtDate", existingTask.CreatedAtDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        int? engineerId = GetUpdatedValue("EngineerId", existingTask.EngineerId, input => int.TryParse(input, out var parsedInt) ? parsedInt : (int?)null);
        string? remarks = GetUpdatedValue("Remarks", existingTask.Remarks);
        DateTime? scheduledDate = GetUpdatedValue("ScheduledDate", existingTask.ScheduledDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? startDate = GetUpdatedValue("StartDate", existingTask.StartDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? deadlineDate = GetUpdatedValue("DeadlineDate", existingTask.DeadlineDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);
        DateTime? completeDate = GetUpdatedValue("CompleteDate", existingTask.CompleteDate, input => DateTime.TryParse(input, out var parsedDateTime) ? parsedDateTime : (DateTime?)null);

        s_dal!.Task.Update(new(existingTask.Id, alias, description, isMilestone, deliverables, level, effortTime, creation, engineerId, remarks, scheduledDate, startDate, deadlineDate, completeDate));
    }

    /// <summary>
    /// Updates an existing dependency based on user input.
    /// </summary>
    /// <exception cref="FormatException">in case the parse didn't succeed.</exception>
    /// <exception cref="DalDoesNotExistException">When the dependency with this ID is not found in the list.</exception>
    static void UpdateDependency()
    {
        // Get the ID from the user
        Console.Write("Enter the Dependency ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var dependencyId))
            throw new FormatException("Invalid input. Please enter a valid Task ID.");
        DO.Dependency? existingDependency = s_dal.Dependency.Read(dependencyId);
        if (existingDependency is not null)
            Console.WriteLine(existingDependency);
        else
            throw new DalDoesNotExistException($"Task with ID={dependencyId} does not exist.");

        // Update dependency properties
        int dependent = GetUpdatedValue("DependentTask", existingDependency.DependentTask, input => int.TryParse(input, out var parsedInt) ? parsedInt : existingDependency.DependentTask);
        int previous = GetUpdatedValue("PreviousTask", existingDependency.PreviousTask, input => int.TryParse(input, out var parsedInt) ? parsedInt : existingDependency.PreviousTask);

        s_dal!.Dependency.Update(new(existingDependency.Id, dependent, previous));
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
    /// The main of the program. Only operating the initialization and the menu.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        if (ans == "Y" || ans == "y") //stage 3
            //Initialization.Do(s_dal); //stage 2
            Initialization.Do(); //stage 4

        Menu();
    }
}