using Core.DTOs;
using Core.DTOs.Data;
using Core.Enums;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Path = System.Windows.Shapes.Path;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    /// <summary>
    /// TODO
    /// </summary>
    internal abstract class SearchResultViewModel : ViewModelBase
    {
        protected readonly LogDataDTO _logDataDTO;
        protected readonly ReportDeskViewModel _reportDesk;
        protected readonly NavigationService _navigationService;
        public abstract LogContext SearchResultContext { get; }


        public abstract ICommand OpenUpdaterCommand { get; }
        public ICommand ViewLogCommand { get; }
        public ICommand DeleteLogCommand { get; }


        public SearchResultViewModel(NavigationService navigationService, IDataService dataService, LogDataDTO logDataDTO, 
            ReportDeskViewModel reportDeskViewModel)
        {
            _logDataDTO = logDataDTO;
            _reportDesk = reportDeskViewModel;
            _navigationService = navigationService;

            ViewLogCommand = new OpenViewerCommand(navigationService, _logDataDTO);
            DeleteLogCommand = new DeleteLogCommand(dataService, reportDeskViewModel, _logDataDTO.LogId, this);
        }



        #region Properties



        public Style? IconStyle { get; init; }

        // For JetBrains (gradient)
        public Path IconPath { get; init; } = new();

        public UIElement IconContainer { get; init; } = new();

        public string Date => _logDataDTO.Start.ToString("d MMMM yyyy");

        public string Project => _logDataDTO.Project.ProjectName ?? "Unknown Project";


        public string Subject => CreateSubject();

        public string Content => CreateContent();






        #endregion




        #region Methods



        public string CreateContent()
        {
            foreach (PostItDTO p in _logDataDTO.PostIts)
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
            const string defaultSubject = "Nothing to Display";

            try
            {
                return _logDataDTO.PostIts.Count > 0 ? _logDataDTO.PostIts[0].Subject.SubjectName : defaultSubject;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near CreateSubject(): {ex.Message}");
            }

            return defaultSubject;
        }





        #endregion

    }
}
