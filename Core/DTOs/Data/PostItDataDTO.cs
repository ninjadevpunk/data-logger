namespace Core.DTOs.Data
{
    public record PostItDataDTO : PostItDTO
    {
        public int UserId { get; set; }
        public int LogId { get; set; }
        public int PostItId { get; set; }
        public int SubjectId { get; set; }
        
        public DateTime ERCaptureTime { get; init; }
        public DateTime SOCaptureTime { get; init; }
    }
}
