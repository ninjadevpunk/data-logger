using Core.Enums;
using Core.Models;
using Core.Utilities;
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
using Path = System.Windows.Shapes.Path;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    /// <summary>
    /// TODO
    /// </summary>
    internal abstract class SearchResultViewModel : ViewModelBase
    {
        protected readonly LOG _LOG;
        protected readonly ReportDeskViewModel _reportDesk;
        protected readonly NavigationService _navigationService;
        public abstract LogContext SearchResultContext { get; }

        public SearchResultViewModel(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService)
        {
            _LOG = log;
            _reportDesk = reportDeskViewModel;
            _navigationService = navigationService;
        }



        #region Properties



        public int ViewModelID => _LOG.ID;

        public Style? IconStyle { get; set; }

        // For JetBrains (gradient)
        public Path IconPath { get; set; } = new();

        public UIElement IconContainer { get; set; } = new();

        public string Date => _LOG.Start.ToString("d MMMM yyyy");

        public string Project => _LOG?.Project?.Name ?? "Unknown Project";


        public string Subject => CreateSubject();

        public string Content => CreateContent();



        #region Commands


        public ICommand ViewLogCommand { get; set; } = null!;

        public ICommand EditLogCommand { get; set; } = null!;

        public ICommand DeleteLogCommand { get; set; } = null!;


        #endregion





        #endregion




        #region Methods




        public string ConvertRtfToPlainText(string rtfContent)
        {
            try
            {
                // CreateViewerViewModel a temporary RichTextBox
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
                if (RtfConverter.HasContent(p.Error))
                {
                    return RtfConverter.ToPlainText(p.Error).Text;
                }
                if (RtfConverter.HasContent(p.Solution))
                {
                    return RtfConverter.ToPlainText(p.Solution).Text;
                }
                if (RtfConverter.HasContent(p.Suggestion))
                {
                    return RtfConverter.ToPlainText(p.Suggestion).Text;
                }
                if (RtfConverter.HasContent(p.Comment))
                {
                    return RtfConverter.ToPlainText(p.Comment).Text;
                }
            }

            return "There are no post its in this log.";
        }


        public string CreateSubject()
        {
            // TODO
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





        #endregion

    }
}
