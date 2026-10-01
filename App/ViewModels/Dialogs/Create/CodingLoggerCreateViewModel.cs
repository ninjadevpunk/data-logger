using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    /// <summary>
    /// Base class for UI concerns on creating cache logs.
    /// </summary>
    internal abstract class CodingLoggerCreateViewModel : CodingCacheLoggerViewModel
    {

        /// <summary>
        /// Create a log that is sent to cache for 20 mins before database store.
        /// </summary>
        public abstract ICommand AnnotateCommand { get; }


        public CodingLoggerCreateViewModel(NavigationService navigationService, IAuthService authService,
            IDataService dataService, ICacheService cacheService)
            : base(navigationService, authService, dataService, cacheService)
        {
        }
    }
}
