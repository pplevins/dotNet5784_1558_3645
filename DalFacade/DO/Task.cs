namespace DO
{
    /// <summary>
    /// Task Entity represents a task with all its properties
    /// </summary>
    /// <param name="Id">Unique identifier for the task (created automatically)</param>
    /// <param name="Alias">Alias of the task</param>
    /// <param name="Description">Description of the task</param>
    /// <param name="IsMilestone">if its a milestone or not</param>
    /// <param name="Deliverables">Deliverables of the task</param>
    /// <param name="DifficultyLevel">Difficulty level of the task</param>
    /// <param name="RequiredEffortTime">Required numbers of days to complete the task/param>
    /// <param name="CreatedAtDate">The date the task created at</param>
    /// <param name="EngineerId">Identifier of the engineer assigned to the task</param>
    /// <param name="Remarks">Additional remarks related to the task</param>
    public record Task
    (
        int Id,
        string Alias,
        string Description,
        bool IsMilestone,
        string Deliverables,
        EngineerExperience DifficultyLevel,
        TimeSpan? RequiredEffortTime = null,
        DateTime? CreatedAtDate = null,
        int? EngineerId = null,
        string? Remarks = null
    )
    {
        /// <summary>
        /// Scheduled start date of the task
        /// </summary>
        public DateTime? ScheduledDate { get; init; }

        /// <summary>
        /// Actual start date of the task
        /// </summary>
        public DateTime? StartDate { get; init; }

        /// <summary>
        /// Planned deadline date of the task
        /// </summary>
        public DateTime? DeadlineDate { get; init; }

        /// <summary>
        /// Actual Completion date of the task
        /// </summary>
        public DateTime? CompleteDate { get; init; }

        /// <summary>
        /// Empty constructor for Task record.
        /// </summary>
        public Task() : this(0, "", "", false, "", EngineerExperience.Beginner, TimeSpan.Zero, null, null, null) { }
    }
}
