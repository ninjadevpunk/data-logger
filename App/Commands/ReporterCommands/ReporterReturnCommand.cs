using Core.Enums;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Returns the user to the log cache.
    /// </summary>
    internal class ReporterReturnCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogContext _context;

        public ReporterReturnCommand(NavigationService navigationService, LogContext logContext)
        {
            _navigationService = navigationService;
            _context = logContext;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToLogCachePage(_context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near DashboardCommand.Execute(): {ex.Message}");
            }
        }
    }

}
