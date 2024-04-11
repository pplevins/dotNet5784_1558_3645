namespace Dal;
/// <summary>
/// Static class containing data sources and configuration settings for the application.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// Configuration settings for the data source.
    /// </summary>
    internal static class Config
    {
        /// <summary>
        /// The starting identifier for tasks.
        /// </summary>
        internal const int startTaskId = 1;
        /// <summary>
        /// Gets the next available identifier for tasks and increments it.
        /// </summary>
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        internal static void ResetTaskId()
        {
            nextTaskId = startTaskId;
        }
        /// <summary>
        /// The starting identifier for dependencies.
        /// </summary>
        internal const int startDependencyId = 1;
        /// <summary>
        /// Gets the next available identifier for dependencies and increments it.
        /// </summary>
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        internal static void ResetDependencyId()
        {
            nextDependencyId = startDependencyId;
        }

        /// <summary>
        /// The start date of the project.
        /// </summary>
        internal static DateTime? ProjectStartDate = null;
        /// <summary>
        /// The scheduled end date of the project.
        /// </summary>
        internal static DateTime? ProjectScheduledEndDate = null;
    }
    /// <summary>
    /// List of Users in the data source.
    /// </summary>
    internal static List<DO.User> Users { get; } = new();
    /// <summary>
    /// List of engineers in the data source.
    /// </summary>
    internal static List<DO.Engineer> Engineers { get; } = new();
    /// <summary>
    /// List of tasks in the data source.
    /// </summary>
    internal static List<DO.Task> Tasks { get; } = new();
    /// <summary>
    /// List of dependencies in the data source.
    /// </summary>
    internal static List<DO.Dependency> Dependencies { get; } = new();
}
