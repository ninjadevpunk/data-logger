namespace Core.DTOs.Data
{
    public abstract record LogDataDTO : LogDTO
    {
        public int LogId { get; init; }
        public abstract int AppId { get; protected init; }
        public int ProjectId { get; init; }
    }
}
