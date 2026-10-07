using MVVMEssentials.Commands;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data.Interfaces;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Deletes a log in the database from the Reporter Desk.
    /// </summary>
    internal class DeleteLogCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly int _logId;


        public DeleteLogCommand(IDataService dataService, ReportDeskViewModel reportDeskViewModel, int logId)
        {
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _logId = logId;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (await _dataService.DeleteLOGByIDAsync(_logId))
                {
                    await _reportDeskViewModel.UpdateLogsListAsync();
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }

}
