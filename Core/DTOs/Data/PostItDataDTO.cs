namespace Core.DTOs.Data
{
    public sealed record PostItDataDTO : PostItDTO
    {
        public int LogId { get; init; }
        public int PostItId { get; init; }
        public int SubjectId { get; init; }
    }
}
