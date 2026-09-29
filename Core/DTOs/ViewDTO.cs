namespace Core.DTOs
{
    public abstract record ViewDTO
    {
        public string ApplicationName { get; init; } = string.Empty;
        public string ProjectName { get; init; } = string.Empty;
        public string Date { get; init; } = string.Empty;

        public string OutputName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;

        public IReadOnlyList<PostItDTO> PostIts { get; init; } = [];
    }
}
