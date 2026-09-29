using Core.Enums;

namespace Core.DTOs.Cache
{
    public record CodingCacheDTO : LogCacheDTO
    {
        public override LogContext Context => LogContext.Coding;
        public override string ApplicationName { get; init; } = string.Empty;
        public int Bugs { get; init; }
        public bool Success { get; init; }
    }
}
