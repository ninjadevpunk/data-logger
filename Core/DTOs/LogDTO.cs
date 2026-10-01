using Core.Enums;

namespace Core.DTOs
{
    public abstract record LogDTO<TPostItList>
    {
        public abstract LogContext Context { get; }

        public abstract string ApplicationName { get; init; }
        public string ProjectName { get; init; } = string.Empty;

        public string Start { get; init; } = string.Empty;
        public string End { get; init; } = string.Empty;

        public int OutputID { get; init; }
        public string OutputName { get; init; } = string.Empty;

        public int TypeID { get; init; }
        public string TypeName { get; init; } = string.Empty;

        public abstract IReadOnlyList<TPostItList> PostIts { get; init; }
    }
}
