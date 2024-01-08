namespace DO
{
    /// <summary>
    /// Task Entity represents a task with all its properties
    /// </summary>
    /// <param name="Id">Unique identifier for the task (created automatically)</param>
    /// <param name="Alias">Alias of the task</param>
    /// <param name="Description">Description of the task</param>
    /// <param name="IsMilestone">if it's a milestone or not</param>
    /// <param name="Deliverables">Deliverables of the task</param>
    /// <param name="DifficultyLevel">Difficulty level of the task</param>
    /// <param name="RequiredEffortTime">Required numbers of days to complete the task</param>
    /// <param name="CreatedAtDate">The date the task created at</param>
    /// <param name="EngineerId">Identifier of the engineer assigned to the task</param>
    /// <param name="Remarks">Additional remarks related to the task</param>
    /// <param name="ScheduledDate">Scheduled start date of the task</param>
    /// <param name="StartDate">Actual start date of the task</param>
    /// <param name="DeadlineDate">Planned deadline date of the task</param>
    /// <param name="CompleteDate">Actual Completion date of the task</param>
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
        string? Remarks = null,
        DateTime? ScheduledDate = null,
        DateTime? StartDate = null,
        DateTime? DeadlineDate = null,
        DateTime? CompleteDate = null
    )
    {
        /// <summary>
        /// Initializes an empty instance of the Task record for stage 3, as instructed.
        /// We opted not to create a parameterized constructor since the record type already has one.
        /// </summary>
        public Task() : this(0, "", "", false, "", EngineerExperience.Beginner, TimeSpan.Zero, null, null, null) { }
    }
}
