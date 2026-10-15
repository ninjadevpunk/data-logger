using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    /// <summary>
    /// This class will be used to update database logs from the log editor.
    /// </summary>
    internal class OpenUpdaterCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly LogDataDTO _logDataDTO;



        public OpenUpdaterCommand(NavigationService navigationService, ReportDeskViewModel reportDeskViewModel, IDataService dataService, LogDataDTO logDataDTO)
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
                await _navigationService.NavigateToUpdater(_reportDeskViewModel, _logDataDTO);

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
