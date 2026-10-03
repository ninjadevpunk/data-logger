using Core.Enums;

namespace Core.DTOs.Cache
{
    public record CodingCacheDTO : LogCacheDTO
    {
        public override LogContext Context => LogContext.Coding;
        public override string ApplicationName { get; init; } = string.Empty;

        /// <summary>
        /// The number of bugs mentioned or associated with the log.
        /// </summary>
        public int Bugs { get; init; }

        /// <summary>
        /// Returns whether or not
        /// </summary>
        public bool Success { get; init; }
    }
}
