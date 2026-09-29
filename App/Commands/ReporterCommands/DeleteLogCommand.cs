using MVVMEssentials.Commands;
using System.Diagnostics;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data.Interfaces;
using Core.Models;
using FileSys.Interfaces;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Deletes a log in the database from the Reporter Desk.
    /// </summary>
    public class DeleteLogCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly LOG _log;
        private LogContext _cacheContext;

        public DeleteLogCommand()
        {
        }

        public DeleteLogCommand(LOG log)
        {
            _log = log;
        }

        public DeleteLogCommand(IDataService dataService, LOG log)
        {
            _log = log;
            _dataService = dataService;
        }

        public DeleteLogCommand(IDataService dataService, LogContext context, LOG log)
        {
            _dataService = dataService;
            _cacheContext = context;
            _log = log;
        }

        public DeleteLogCommand(IDataService dataService, ReportDeskViewModel reportDeskViewModel, LogContext context, LOG log)
        {
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _cacheContext = context;
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
