namespace Migration.Models
{
    public sealed class MigrationIssue
    {
        public string EntityType { get; init; } = string.Empty;
        public int EntityId { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}
