using MVVMEssentials.Commands;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data.Interfaces;
using System.DirectoryServices;
using Data_Logger_1._3.ViewModels.Reporter.Result;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Deletes a log in the database from the Reporter Desk.
    /// </summary>
    internal class DeleteLogCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly SearchResultViewModel? _searchResultViewModel;
        private readonly int _logId;


        public DeleteLogCommand(IDataService dataService, ReportDeskViewModel reportDeskViewModel, int logId)
        {
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _logId = logId;
        }

        public DeleteLogCommand(IDataService dataService, ReportDeskViewModel reportDeskViewModel, int logId, 
            SearchResultViewModel searchResultViewModel)
        {
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _logId = logId;
            _searchResultViewModel = searchResultViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (await _dataService.DeleteLOGByIDAsync(_logId))
                {
                    await _reportDeskViewModel.UpdateLogsListAsync();
                    _reportDeskViewModel.SearchCommand.Execute(_searchResultViewModel);
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }

}
