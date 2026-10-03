using Core.Enums;

namespace Core.DTOs
{
    public abstract record LogDTO
    {
        /// <summary>
        /// Log's context.
        /// </summary>
        public abstract LogContext Context { get; }

        /// <summary>
        /// Logged-in user's ID.
        /// </summary>
        public int UserId { get; init; }

        /// <summary>
        /// The application name.
        /// </summary>
        public abstract string ApplicationName { get; init; }

        /// <summary>
        /// The project's name.
        /// </summary>
        public string ProjectName { get; init; } = string.Empty;

        /// <summary>
        /// The date the log is created.
        /// </summary>
        public DateTime Start { get; init; }

        /// <summary>
        /// The date the log is modified.
        /// </summary>
        public DateTime End { get; init; }

        public int OutputID { get; init; }

        /// <summary>
        /// The output name.
        /// </summary>
        public string OutputName { get; init; } = string.Empty;

        public int TypeID { get; init; }
        /// <summary>
        /// The type name.
        /// </summary>
        public string TypeName { get; init; } = string.Empty;

        /// <summary>
        /// List of cache/data PostIts.
        /// </summary>
        public IReadOnlyList<PostItDTO> PostIts { get; init; } = [];
    }
}
