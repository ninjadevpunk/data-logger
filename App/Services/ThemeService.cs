using Core.Models;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    internal sealed class ThemeService
    {
        private const string ThemeFolder = "/Design;component/Themes/";

        public void ApplyTheme(Theme theme)
        {
            var app = Application.Current ?? throw new InvalidOperationException("WPF application is not available.");

            var dictionaries = app.Resources.MergedDictionaries;

            string themeFile = theme switch
            {
                Theme.Light => "Light.xaml",
                Theme.Grey => "Grey.xaml",
                _ => "Default.xaml"
            };

            var newDictionary = new ResourceDictionary
            {
                Source = new Uri($"{ThemeFolder}{themeFile}", UriKind.Relative)
            };

            // Find the currently active theme dictionary.
            for (int i = 0; i < dictionaries.Count; i++)
            {
                string? source = dictionaries[i].Source?.OriginalString;

                if (source?.Contains(ThemeFolder, StringComparison.OrdinalIgnoreCase) == true)
                {
                    dictionaries[i] = newDictionary;
                    return;
                }
            }

            // No theme dictionary was merged yet.
            // Insert before the style dictionaries.
            int insertIndex = dictionaries.Count;

            for (int i = 0; i < dictionaries.Count; i++)
            {
                string? source =
                    dictionaries[i].Source?.OriginalString;

                if (source?.Contains(
                    "/Styles/",
                    StringComparison.OrdinalIgnoreCase) == true)
                {
                    insertIndex = i;
                    break;
                }
            }

            dictionaries.Insert(insertIndex, newDictionary);
        }
    }

}
