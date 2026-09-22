using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Returns the user to the log cache.
    /// </summary>
    public class ReporterReturnCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        public CacheContext Context { get; set; } = CacheContext.Coding;
        public ReporterReturnCommand(NavigationService navigationService, CacheContext cacheContext)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                Context = cacheContext;
            }
            catch (ArgumentNullException nullex)
            {
                Debug.WriteLine($"ArgumentNullException occurred near DashboardCommand constructor: {nullex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToLogCachePage(Context);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred near DashboardCommand.Execute(): {e.Message}");
            }
        }
    }

}
