using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static Data_Logger_1._3.Services.CacheMaster;
using Path = System.Windows.Shapes.Path;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    /// <summary>
    /// The base class for all search reult items.
    /// </summary>
    public abstract class SearchResultViewModel : ViewModelBase
    {
        protected readonly LOG _LOG;
        protected readonly ReportDeskViewModel _reportDesk;
        protected readonly NavigationService _navigationService;
        public abstract CacheContext SearchResultContext { get; }

        public SearchResultViewModel(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService)
        {
            _LOG = log;
            _reportDesk = reportDeskViewModel;
            _navigationService = navigationService;


        }



        #region Properties



        public int ViewModelID => _LOG.ID;

        public Style IconStyle { get; set; }

        // For JetBrains (gradient)
        public Path IconPath { get; set; }

        public UIElement IconContainer { get; set; }

        public string Date => _LOG.Start.ToString("d MMMM yyyy");

        public string Project => _LOG?.Project?.Name ?? "Unknown Project";


        public string Subject => CreateSubject();

        public string Content => CreateContent();



        #region Commands


        public ICommand ViewLogCommand { get; set; }

        public ICommand EditLogCommand { get; set; }

        public ICommand DeleteLogCommand { get; set; }


        #endregion





        #endregion




        #region Methods




        public string ConvertRtfToPlainText(string rtfContent)
        {
            try
            {
                // Create a temporary RichTextBox
                var richTextBox = new RichTextBox();

                // Convert RTF content to a byte array
                byte[] rtfBytes = Encoding.UTF8.GetBytes(rtfContent);

                // Use a MemoryStream to load the RTF content
                using (var stream = new MemoryStream(rtfBytes))
                {
                    richTextBox.Document = new FlowDocument();
                    var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    textRange.Load(stream, DataFormats.Rtf);
                }

                // Extract plain text
                string plainText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                return plainText;
            }
            catch (ArgumentException argx)
            {
                Debug.WriteLine($"ArgumentException near ConvertRtfToPlaintText(): {argx.Message}");

                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near ConvertRtfToPlainText(): {ex.Message}");

                // TODO

                return string.Empty;
            }
        }


        public string CreateContent()
        {
            foreach (PostIt p in _LOG.PostItList)
            {
                if (!string.IsNullOrEmpty(ConvertRtfToPlainText(p.Error)))
                {
                    return ConvertRtfToPlainText(p.Error);
                }
                if (!string.IsNullOrEmpty(ConvertRtfToPlainText(p.Solution)))
                {
                    return ConvertRtfToPlainText(p.Solution);
                }
                if (!string.IsNullOrEmpty(ConvertRtfToPlainText(p.Suggestion)))
                {
                    return ConvertRtfToPlainText(p.Suggestion);
                }
                if (!string.IsNullOrEmpty(ConvertRtfToPlainText(p.Comment)))
                {
                    return ConvertRtfToPlainText(p.Comment);
                }
            }

            return "There are no post its in this log.";
        }


        public string CreateSubject()
        {
            try
            {
                return _LOG.PostItList is not null && _LOG.PostItList.Count > 0 ? _LOG.PostItList[0].Subject.Subject : "Nothing to Display";
            }
            catch (NullReferenceException nullex)
            {
                Debug.WriteLine($"NullReferenceException occurred near CreateSubject(): {nullex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near CreateSubject(): {ex.Message}");
            }

            return "Nothing to Display";
        }

        protected static Style? TryParsePath(string value)
        {
            try
            {
                return (Style)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException rex)
            {
                Debug.WriteLine($"ResourceReferenceKeyNotFoundException found near qt_SearchResultViewModel.TryParseBrush(): {rex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near qt_SearchResultViewModel.TryParseBrush(): {ex.Message}");
                return null;
            }
        }




        #endregion

    }
}
