using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    /// <summary>
    /// Base class for UI concerns on creating cache logs.
    /// </summary>
    internal abstract class LoggerCreateViewModel : LoggerViewModel
    {
        public override LogContext Context { get; }
        public override LOG.CATEGORY Category { get; }
        protected readonly ICacheService _cacheService;

        /// <summary>
        /// Create a log that is sent to cache for 20 mins before database store.
        /// </summary>
        public abstract ICommand AnnotateCommand { get; }



        public LoggerCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IAuthService authService,
            IDataService dataService, ICacheService cacheService)
            : base(navigationService, logCacheViewModel, authService, dataService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService), "LoggerCreateViewModel");
        }
    }
}
