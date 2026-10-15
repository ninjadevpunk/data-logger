using Core.DTOs.Data;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    /// <summary>
    /// The base class for all REPORT items to be shown in a ReportDeskViewModel.
    /// </summary>
    internal abstract class REPORTViewModel : ViewModelBase
    {
        private readonly LogDataDTO _logDataDTO;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        protected readonly IDataService _dataService;

        public abstract ICommand SingleExport { get; }
        public abstract ICommand OpenUpdaterCommand { get; }
        public ICommand OpenViewerCommand { get; } = null!;
        public ICommand DeleteLogCommand { get; }


        public REPORTViewModel(IDataService dataService, ReportDeskViewModel reportDeskViewModel, LogDataDTO logDataDTO)
        {
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _logDataDTO = logDataDTO;

            //SingleExportCommand
            //OpenViewerCommand
            DeleteLogCommand = new DeleteLogCommand(_dataService, _reportDeskViewModel, _logDataDTO.LogId);
        }



        #region Properties





        /// <summary>
        /// The project name of the log
        /// </summary>
        public string ProjectName => $"{_logDataDTO.Project.ProjectName} ({_logDataDTO.ApplicationName})";


        /// <summary>
        /// How many errors are in the log
        /// </summary>
        public int ErrorCount => _logDataDTO.PostIts.Count(p => RtfConverter.HasContent(p.Error));
        /// <summary>
        /// How many solutions are in the log.
        /// </summary>
        public int SolutionCount => _logDataDTO.PostIts.Count(p => RtfConverter.HasContent(p.Solution));
        /// <summary>
        /// How many suggestions are in the log.
        /// </summary>
        public int SuggestionCount => _logDataDTO.PostIts.Count(p => RtfConverter.HasContent(p.Suggestion));
        /// <summary>
        /// How many comments are in the log.
        /// </summary>
        public int CommentCount => _logDataDTO.PostIts.Count(p => RtfConverter.HasContent(p.Comment));

        /// <summary>
        /// Formatted start and end Date.
        /// </summary>
        public string StartEndDate => $"{_logDataDTO.Start:dddd, d MMMM yyyy HH:mm:ss.fff} - " +
            $"{_logDataDTO.End:dddd, d MMMM yyyy HH:mm:ss.fff}";

        /// <summary>
        /// Store the first occurence of a note with acceptable input only.
        /// </summary>
        public string NotaryContent
        {
            get
            {
                foreach (var postIt in _logDataDTO.PostIts)
                {
                    var rtfFields = new[] { postIt.Error, postIt.Solution, postIt.Suggestion, postIt.Comment };

                    foreach (var rtf in rtfFields)
                    {
                        var (text, exception) = RtfConverter.ToPlainText(rtf);

                        if (exception != null)
                        {
                            _dataService.HandleExceptionAsync(exception);
                            Debug.WriteLine($"{Constants.ExceptionPrefix} LOGViewModel NotaryContent getter: {exception.Message}");
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            return text;
                        }
                    }
                }

                return "No Notes";
            }
        }





        #endregion


    }
}
