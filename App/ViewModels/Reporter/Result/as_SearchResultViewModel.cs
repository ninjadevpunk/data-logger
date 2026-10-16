using Core.DTOs.Data;
using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    internal class as_SearchResultViewModel : SearchResultViewModel
    {
        public override LogContext SearchResultContext => LogContext.AndroidStudio;
        public override ICommand OpenUpdaterCommand { get; }

        public as_SearchResultViewModel(NavigationService navigationService, IDataService dataService, LogDataDTO logDataDTO, 
            ASReportDeskViewModel androidReportDeskViewModel) : 
            base(navigationService, dataService, logDataDTO, androidReportDeskViewModel)
        {
            var style = Extensions.TryParsePath("path_AndroidStudio_icon");
            IconStyle = style ?? new Style();

            OpenUpdaterCommand = new OpenUpdaterCommand(navigationService, androidReportDeskViewModel, dataService, logDataDTO);
        }
    }
}
