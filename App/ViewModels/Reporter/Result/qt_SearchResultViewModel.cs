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
    /// <summary>
    /// TODO
    /// </summary>
    internal class qt_SearchResultViewModel : SearchResultViewModel
    {
        public override LogContext SearchResultContext => LogContext.Qt;
        public override ICommand OpenUpdaterCommand { get; }

        public qt_SearchResultViewModel(NavigationService navigationService, IDataService dataService, QtDataDTO qtDataDTO, 
            QtReportDeskViewModel qtReportDeskViewModel) : 
            base(navigationService, dataService, qtDataDTO, qtReportDeskViewModel)
        {
            var style = Extensions.TryParsePath("path_Qt_icon");
            IconStyle = style ?? new Style();

            OpenUpdaterCommand = new OpenUpdaterCommand(navigationService, qtReportDeskViewModel, dataService, qtDataDTO);
        }



    }
}
