namespace Core.DTOs.Cache
{
    public abstract record LogCacheDTO : LogDTO<PostItCacheDTO>
    {
        public int UserId { get; init; }

        public override IReadOnlyList<PostItCacheDTO> PostIts { get; init; } = [];
    }
}
