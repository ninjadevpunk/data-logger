using Core.Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Data_Logger_1._3.Utilities
{
    internal static class Extensions
    {
        /// <summary>
        /// Indicates a green colour for positive status messages.
        /// </summary>
        internal static Brush MessageGood => Extensions.TryParseBrush("GREEN");

        /// <summary>
        /// Indicates a red colour for negative status messages.
        /// </summary>
        internal static Brush MessageBad => Extensions.TryParseBrush("RED");

        /// <summary>
        /// Finds a <see cref="Brush"/> resource in App.xaml.
        /// </summary>
        /// <param name="value">Resource value</param>
        /// <returns>Returns a Brush</returns>
        public static Brush TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException rex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} TryParseBrush: {rex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} TryParseBrush: {ex.Message}");
            }

            return Brushes.Transparent;
        }

        /// <summary>
        /// Finds a path <see cref="Style"/> resource in App.xaml.
        /// </summary>
        /// <param name="value">Resource value</param>
        /// <returns>Returns a Brush</returns>
        public static Style? TryParsePath(string value)
        {
            try
            {
                return (Style)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException rex)
            {
                Debug.WriteLine($"ResourceReferenceKeyNotFoundException occurred in TryParsePath: {rex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} TryParsePath: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Gets the number of coding logs in the cache list.
        /// </summary>
        /// <param name="dataService">DataService dependency</param>
        /// <param name="codingViewModel">Cache list ViewModel</param>
        /// <returns>Count of coding logs</returns>
        public static async Task<string> GetLogCountAsync(IDataService dataService, CodingViewModel codingViewModel)
        {
            var count = await dataService.LogCountAsync(LOG.CATEGORY.CODING);
            codingViewModel.LogCount = $"{codingViewModel.CacheItems.Count} coding logs cached | {count} total logs";

            return $"{codingViewModel.CacheItems.Count} Logs Cached";
        }


        /// <summary>
        /// Gets the number of Android Studio logs in the cache list.
        /// </summary>
        /// <param name="dataService">DataService dependency</param>
        /// <param name="codingAndroidViewModel">Android Studio cache list ViewModel</param>
        /// <returns>Count of Android Studio coding logs</returns>
        public static async Task<string> GetAndroidLogCountAsync(IDataService dataService, CodingAndroidViewModel codingAndroidViewModel)
        {
            var count = await dataService.ASLogCountAsync();
            codingAndroidViewModel.LogCount = $"{codingAndroidViewModel.CacheItems.Count} android logs cached | {count} total logs";
            return $"{codingAndroidViewModel.CacheItems.Count} Logs Cached";
        }
    }
}
