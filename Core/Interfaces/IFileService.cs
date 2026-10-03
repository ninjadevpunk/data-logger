using Core.Models;

namespace Core.Interfaces
{
    public interface IFileService
    {
        bool CreateDirectory(string path);
        bool Exists(string path);
        string? ReadAllText(string path);
        bool DeleteDirectory(string path);

        /// <summary>
        /// Creates vital resources for the app to function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        bool ResourcesCreated();

    }
}
