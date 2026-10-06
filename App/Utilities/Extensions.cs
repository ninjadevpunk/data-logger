using Core.DTOs.Cache;
using Core.DTOs.Identity;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Data_Logger_1._3.Utilities
{
    internal static class Extensions
    {
        /// <summary>
        /// Turns user settings into a UserDTO.
        /// </summary>
        /// <param name="userSettings">The current user settings</param>
        /// <returns>A UserDTO</returns>
        internal static UserDTO UserSettingsToDTO(UserSettings userSettings)
        {
            return new UserDTO
            {
                UserId = userSettings.Id,
                ProfilePic = userSettings.ProfilePic,
                Name = userSettings.Name,
                Surname = userSettings.Surname,
                Email = userSettings.Email,
                IsEmployee = userSettings.IsCompanyEmployee,
                CompanyName = userSettings.CompanyName,
                CompanyAddress = userSettings.CompanyAddress,
                CompanyLogo = userSettings.CompanyLogo,
            };
        }


        #region App.xaml




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
        internal static Brush TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException rex)
            {
                Debug.WriteLine($"ResourceReferenceKeyNotFoundException occurred in TryParseBrush: {rex.Message}");
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
        internal static Style? TryParsePath(string value)
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




        #endregion




        #region PostIts




        /// <summary>
        /// Validates PostIt input before a PostIt is created or edited.
        /// </summary>
        /// <param name="subject">The subject of the PostIt.</param>
        /// <param name="error">The RTF-formatted error content.</param>
        /// <param name="solution">The RTF-formatted solution content.</param>
        /// <param name="suggestion">The RTF-formatted suggestion content.</param>
        /// <param name="comment">The RTF-formatted comment content.</param>
        /// <returns>
        /// <see langword="true"/> when the subject is provided and at least one content
        /// field contains readable text; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool ValidatePostIt(string subject, string error, string solution, string suggestion, string comment)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            bool allAreEmpty = !RtfConverter.HasContent(error) && !RtfConverter.HasContent(solution) && !RtfConverter.HasContent(suggestion) 
                && !RtfConverter.HasContent(comment);

            if (allAreEmpty)
            {
                MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a <see cref="PostItCacheDTO"/> from the supplied PostIt data,
        /// converting the relevant RTF content to plain text and assigning capture times.
        /// </summary>
        /// <param name="subject">The subject of the PostIt.</param>
        /// <param name="error">The RTF-formatted error content.</param>
        /// <param name="solution">The RTF-formatted solution content.</param>
        /// <param name="suggestion">The RTF-formatted suggestion content.</param>
        /// <param name="comment">The RTF-formatted comment content.</param>
        /// <returns>A populated <see cref="PostItCacheDTO"/> containing the PostIt data.</returns>
        /// <exception cref="Exception">Thrown when the error or solution RTF cannot be converted to plain text.</exception>
        internal static PostItCacheDTO BuildPostItCacheDTO(string subject, string error, string solution, string suggestion, string comment)
        {
            var (errorPlaintext, errorException) = RtfConverter.ToPlainText(error);
            var (solutionPlaintext, solutionException) = RtfConverter.ToPlainText(solution);

            if (errorException != null)
                throw errorException;

            if (solutionException != null)
                throw solutionException;

            return new PostItCacheDTO
            {
                Subject = subject,
                Error = ValidatePostItRtf(error),
                Solution = ValidatePostItRtf(solution),
                Suggestion = ValidatePostItRtf(suggestion),
                Comment = ValidatePostItRtf(comment),
                ERCaptureTime = IsEmpty(errorPlaintext) ? default : DateTime.Now,
                SOCaptureTime = IsEmpty(solutionPlaintext) ? default : DateTime.Now
            };
        }


        /// <summary>
        /// Takes normal RTF, checks if it has content and returns it if it not white space. Useful for removing 
        /// formatting marks and other artifacts left over in plain text.
        /// </summary>
        /// <param name="rtf">Raw RTF</param>
        /// <returns>RTF if it has content; otherwise empty <see cref="string"/></returns>
        internal static string ValidatePostItRtf(string rtf) => RtfConverter.HasContent(rtf) ? rtf : string.Empty;

        /// <summary>
        /// Checks if plain text is white space or not.
        /// </summary>
        /// <param name="fieldText">Plain text</param>
        /// <returns>False if the field is null or white space; otherwise true</returns>
        internal static bool IsEmpty(string fieldText) => string.IsNullOrWhiteSpace(fieldText);




        #endregion





        #region Notes



        internal static string NoteDateTimeNow() => DateTime.Now.ToString("d MMMM yyyy HH:mm");

        internal static string CreatedAndModifiedDate(string creationDate, string modified)
        {
            modified = creationDate == modified ? string.Empty : $". Last modified on {modified}";
            return $"Created on {creationDate}{modified}";
        }



        #endregion



        #region Cache



        internal static string CreateCacheIdentifier(DateTime logCacheStartTime) => logCacheStartTime.ToString("yyyyMMdd_HHmmss");




        #endregion




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
