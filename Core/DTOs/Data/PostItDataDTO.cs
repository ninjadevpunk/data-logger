namespace Core.DTOs.Data
{
    public record PostItDataDTO : PostItDTO
    {
        public int LogId { get; set; }
        public int PostItId { get; set; }
        public int SubjectId { get; set; }
    }
}
