using MVVMEssentials.Commands;
using System.Diagnostics;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data.Interfaces;
using Core.Models;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Deletes a log in the database from the Reporter Desk.
    /// </summary>
    internal class DeleteLogCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly LOG _log;


        public DeleteLogCommand(IDataService dataService, ReportDeskViewModel reportDeskViewModel, LOG log)
        {
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _log = log;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (await _dataService.DeleteLOGAsync(_log))
                {
                    await _reportDeskViewModel.UpdateLogsListAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred near DeleteLogCommand.Execute(): {e.Message}");
            }
        }
    }

}
