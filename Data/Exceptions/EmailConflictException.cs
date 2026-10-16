namespace Data.Exceptions
{
    public class EmailConflictException : Exception
    {
        public EmailConflictException(string message) : base(message) { }
    }
}
