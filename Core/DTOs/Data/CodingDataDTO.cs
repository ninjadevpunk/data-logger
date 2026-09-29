using Core.Enums;

namespace Core.DTOs.Data
{
    public record CodingDataDTO : LogDataDTO
    {
        public override LogContext Context => LogContext.Coding;
        public override int AppId { get; protected init; }
        public override string ApplicationName { get; init; } = string.Empty;
        public int Bugs { get; init; }
        public bool Success { get; init; }
    }
}
