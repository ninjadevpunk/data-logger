namespace Migration.Models
{
    public sealed class MigrationResult
    {
        public string OldDbPath { get; init; } = string.Empty;
        public string NewDbPath { get; init; } = string.Empty;
        public List<MigrationIssue> Issues { get; } = [];
    }
}
