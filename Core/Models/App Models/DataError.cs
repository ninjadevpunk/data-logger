namespace Core.Models.App_Models
{
    public sealed class DataError
    {
        public string MethodName { get; init; } = string.Empty;
        public Exception? Exception { get; init; }
    }
}
