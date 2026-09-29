using Core.Models.App_Models;

namespace Core.Models
{
    /// <summary>
    /// Represents the result of an operation, including its success state,
    /// optional public message, and any associated exception.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Represents the type of message included in the result.
        /// </summary>
        public enum MessageType
        {
            None, 
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// Gets a value indicating whether the result contains a public message.
        /// </summary>
        public bool HasMessage => !string.IsNullOrWhiteSpace(PublicMessage);

        /// <summary>
        /// Gets or sets the type of the public message.
        /// </summary>
        public MessageType PublicMessageType { get; set; } = MessageType.None;

        /// <summary>
        /// Gets the message intended to be displayed to the user.
        /// </summary>
        public string PublicMessage { get; init; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the operation completed successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Gets the exception associated with the result, if an exception occurred.
        /// </summary>
        public Exception? Exception { get; init; }
    }

}
