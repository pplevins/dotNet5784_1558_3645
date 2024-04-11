namespace DalTest;
using DalApi;
using DO;

/// <summary>
/// Static class responsible for initializing the data source with sample data.
/// </summary>
public static class Initialization
{
    //private static IDependency? s_dalDependency; //stage 1
    //private static IEngineer? s_dalEngineer; //stage 1
    //private static ITask? s_dalTask; //stage 1
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    /// <summary>
    /// Creates sample dependencies and adds them to the data source.
    /// </summary>
    private static void createDependencies()
    {
        var depArr = new[]
        {
            new { Dep = 2, Pre = 1 }, new { Dep = 3, Pre = 1 }, new { Dep = 4, Pre = 1 }, new { Dep = 4, Pre = 2 }, new { Dep = 4, Pre = 3 },
            new { Dep = 5, Pre = 1 }, new { Dep = 5, Pre = 2 }, new { Dep = 6, Pre = 1 }, new { Dep = 6, Pre = 3 }, new { Dep = 6, Pre = 5 },
            new { Dep = 7, Pre = 1 }, new { Dep = 7, Pre = 2 }, new { Dep = 8, Pre = 5 }, new { Dep = 8, Pre = 7 }, new { Dep = 9, Pre = 1 },
            new { Dep = 9, Pre = 3 }, new { Dep = 10, Pre = 1 }, new { Dep = 10, Pre = 9 }, new { Dep = 11, Pre = 8 }, new { Dep = 11, Pre = 9 },
            new { Dep = 11, Pre = 10 }, new { Dep = 12, Pre = 9 }, new { Dep = 12, Pre = 10 }, new { Dep = 13, Pre = 8 }, new { Dep = 13, Pre = 10 },
            new { Dep = 14, Pre = 9 }, new { Dep = 14, Pre = 10 }, new { Dep = 15, Pre = 6 }, new { Dep = 15, Pre = 11 }, new { Dep = 15, Pre = 14 },
            new { Dep = 16, Pre = 1 }, new { Dep = 16, Pre = 3 }, new { Dep = 16, Pre = 5 }, new { Dep = 17, Pre = 9 }, new { Dep = 17, Pre = 12 },
            new { Dep = 17, Pre = 16 }, new { Dep = 18, Pre = 2 }, new { Dep = 18, Pre = 4 }, new { Dep = 18, Pre = 6 }, new { Dep = 19, Pre = 8 },
            new { Dep = 19, Pre = 18 }, new { Dep = 20, Pre = 1 }, new { Dep = 20, Pre = 2 }, new { Dep = 20, Pre = 18 }
        };

        foreach (var dep in depArr)
        {
            int depTask = dep.Dep;
            int preTask = dep.Pre;

            Dependency newDep = new(0, depTask, preTask);

            s_dal!.Dependency.Create(newDep);
        }
    }
    /// <summary>
    /// Creates sample engineers and adds them to the data source.
    /// </summary>
    private static void createEngineers()
    {
        string[] engineerNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var name in engineerNames)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Engineer.Read(id) != null);

            string email = name.Replace(" ", "").ToLower() + "@gmail.com"; //generate email addresses for the initialization.

            EngineerExperience level = (EngineerExperience)(id % 5); //generate random experience level for the engineer

            Engineer newEng = new(id, name, email, level);

            s_dal!.Engineer.Create(newEng);
            createUser(newEng);
        }
    }
    /// <summary>
    /// Creates sample user and adds them to the data source.
    /// </summary>
    private static void createUser(Engineer newEng)
    {
        UserPermission userPermission;
        if (newEng.Name.Equals("Eli Amar")) userPermission = UserPermission.Manager;
        else userPermission = (newEng.Id % 2 == 0) ? UserPermission.Manager : UserPermission.Engineer;
        var new_User = new User(newEng.Id, newEng.Email, userPermission, newEng.Email);
        s_dal!.User.Create(new_User);
    }
    /// <summary>
    /// Creates sample tasks and adds them to the data source.
    /// </summary>
    private static void createTasks()
    {
        var taskArr = new[]
        {
            new { Name = "Complete Coding Assignment", Description = "Finish the coding task for the project", Deliverables = "finished code" },
            new { Name = "Test Software Components", Description = "Verify the functionality of software modules" , Deliverables = "completed tests"},
            new { Name = "Write Test Cases", Description = "Develop test cases for software testing", Deliverables = "finished test cases" },
            new { Name = "Perform Code Review", Description = "Evaluate and provide feedback on code written by team members", Deliverables = "feedbacked code" },
            new { Name = "Debug Code Issues", Description = "Identify and fix bugs in the source code", Deliverables = "debuged code" },
            new { Name = "Implement New Feature", Description = "Add a new functionality to the existing software", Deliverables = "New feature" },
            new { Name = "Optimize Database Queries", Description = "Improve the efficiency of database queries", Deliverables = "Improved database queries" },
            new { Name = "Conduct Research for Project", Description = "Gather information for a specific project aspect", Deliverables = "new information" },
            new { Name = "Write Unit Tests", Description = "Develop unit tests to ensure code quality", Deliverables = "unit tests" },
            new { Name = "Attend Team Meeting", Description = "Participate in a scheduled team discussion", Deliverables = "meeting Participation" },
            new { Name = "Create Project Timeline", Description = "Outline the timeline for project milestones", Deliverables = "project timeline" },
            new { Name = "Review User Feedback", Description = "Examine feedback from users and make improvements", Deliverables = "feedback improvement" },
            new { Name = "Collaborate with Team Members", Description = "Work together with team members on a specific task", Deliverables = "Collaboration work" },
            new { Name = "Review Documentation", Description = "Check and update project documentation", Deliverables = "updated documentation" },
            new { Name = "Document API Endpoints", Description = "Create documentation for the API endpoints", Deliverables = "API endpoints documentation" },
            new { Name = "Create User Interface Mockups", Description = "Design mockups for the user interface", Deliverables = "UI mockup" },
            new { Name = "Prepare Presentation Slides", Description = "Create slides for the upcoming presentation", Deliverables = "Presentation Slides" },
            new { Name = "Troubleshoot Production Issues", Description = "Investigate and resolve issues in the production environment", Deliverables = "resolve issues" },
            new { Name = "Attend Training Session", Description = "Participate in a training session to enhance skills", Deliverables = "training session attendence" },
            new { Name = "Optimize Algorithm Performance", Description = "Improve the efficiency of algorithms in the software", Deliverables = "efficient software" }
        };

        foreach (var task in taskArr)
        {
            string alias = task.Name;
            string description = task.Description;
            string deliverables = task.Deliverables;
            EngineerExperience level = (EngineerExperience)s_rand.Next(5);
            TimeSpan requiredTime = TimeSpan.FromDays(s_rand.Next(1, 5));
            TimeSpan time = new(s_rand.Next(1, 31), 0, 0, 0, 0);
            //DateTime taskCreation = DateTime.Now - time;
            DateTime taskCreation = DateTime.Today - time;

            Task newTask = new(0, alias, description, false, deliverables, level, requiredTime, taskCreation);

            s_dal!.Task.Create(newTask);
        }
    }
    /// <summary>
    /// Performs the initialization process by creating sample data.
    /// </summary>
    /// <param name="dal">The dal interface</param>
    /// <exception cref="NullReferenceException">in case the dal argument is null</exception>
    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {
        //s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");

        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4

        s_dal.Engineer.Reset();
        s_dal.Task.Reset();
        s_dal.Dependency.Reset();
        s_dal.User.Reset();
        s_dal.ProjectStartDate = null;

        createTasks();
        createEngineers();
        createDependencies();
    }
    /// <summary>
    /// Performs the reset operation.
    /// </summary>
    /// <param name="dal">The dal interface</param>
    /// <exception cref="NullReferenceException">in case the dal argument is null</exception>
    //public static void Do(IDal dal) //stage 2
    public static void Reset() //stage 4
    {
        s_dal = DalApi.Factory.Get; //stage 4

        s_dal.Engineer.Reset();
        s_dal.User.Reset();
        s_dal.Task.Reset();
        s_dal.Dependency.Reset();

    }
}
