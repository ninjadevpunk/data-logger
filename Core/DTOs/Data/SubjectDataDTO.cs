namespace Core.DTOs.Data
{
    public sealed record SubjectDataDTO : SubjectDTO
    {
        public int SubjectId { get; init; }
        public int AppId { get; init; }
        public int ProjectId { get; init; }
    }
}
