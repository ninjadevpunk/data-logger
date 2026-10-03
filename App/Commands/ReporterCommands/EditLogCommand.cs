using Core.DTOs.Data;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Allows the user to edit a log retrieved from the database.
    /// </summary>
    internal class EditLogCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly LogDataDTO _logDataDTO;

        

        public EditLogCommand(NavigationService navigationService, IDataService dataService, ReportDeskViewModel reportDeskViewModel, LogDataDTO logDataDTO)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _logDataDTO = logDataDTO;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToUpdater(_logDataDTO, _reportDeskViewModel);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near EditCommand.Execute(): {ex.Message}");
            }
        }
    }

}
