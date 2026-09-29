namespace Core.DTOs.Data
{
    public abstract record LogDataDTO : LogDTO<PostItDataDTO>
    {
        public int UserId { get; init; }
        public int LogId { get; init; }
        public abstract int AppId { get; protected init; }
        public int ProjectId { get; init; }

        public override IReadOnlyList<PostItDataDTO> PostIts { get; init; } = [];
    }
}
