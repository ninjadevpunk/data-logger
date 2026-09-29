namespace Core.DTOs.Cache
{
    public record PostItCacheDTO : PostItDTO
    {
        public DateTime ERCaptureTime { get; init; }
        public DateTime SOCaptureTime { get; init; }
    }
}
