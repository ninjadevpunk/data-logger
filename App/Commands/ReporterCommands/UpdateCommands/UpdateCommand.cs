using Core.DTOs.Data;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    /// <summary>
    /// This class will be used to update database logs from the log editor.
    /// </summary>
    internal class UpdateCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerViewModel;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly LogContext _context = LogContext.Coding;
        private readonly LogDataDTO _logDataDTO;



        public UpdateCommand(NavigationService navigationService, LoggerViewModel loggerCreateViewModel, ReportDeskViewModel reportDeskViewModel,
            IAuthService authService, IDataService dataService, LogDataDTO logDataDTO, LogContext type = LogContext.Coding)
        {
            _context = logDataDTO.Context;
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerViewModel = loggerCreateViewModel;
            _reportDeskViewModel = reportDeskViewModel;
            _logDataDTO = logDataDTO;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {

                await _reportDeskViewModel.UpdateLogsListAsync();

                //await _dataService.UpdateLogAsync(_log);
                await _navigationService.NavigateToReporter();


                return;
            }
            catch (TaskCanceledException taskx)
            {
                await _dataService.HandleExceptionAsync(taskx);
            }
            catch (FormatException formx)
            {
                if (formx.Message.Equals("The input string '' was not in a correct format."))
                {
                    MessageBox.Show($"An error occurred. Please ensure you entered numbers only in fields that require numeric values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

                await _dataService.HandleExceptionAsync(formx);

            }
            catch (Exception ex)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                await _dataService.HandleExceptionAsync(ex);
            }

            _navigationService.GoBack(false);
        }
    }
}
