using System.Windows;

namespace Data_Logger_1._3.Utilities
{
    /// <summary>
    /// UI helper methods and formatters.
    /// </summary>
    internal static class Formatters
    {
        /// <summary>
        /// Returns the correct visibility based on the field's contents. Returns collapsed state if there's null or empty text.
        /// </summary>
        /// <param name="fieldContent">The field's content</param>
        /// <returns>A visibility state</returns>
        static internal Visibility FieldVisible(string fieldContent) => string.IsNullOrWhiteSpace(fieldContent) ? Visibility.Collapsed 
            : Visibility.Visible;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="fieldContent"></param>
        /// <returns></returns>
        static internal double BlockHeight(string fieldContent) => string.IsNullOrWhiteSpace(fieldContent) || fieldContent.Length < 64 ? 24 
            : double.NaN;
    }
}
