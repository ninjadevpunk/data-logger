using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal abstract class CodingLoggerEditViewModel : CodingCacheLoggerViewModel
    {

        /// <summary>
        /// Edits a log that is sent to cache for 20 mins before database store.
        /// </summary>
        public abstract ICommand CacheEditCommand { get; }

        public CodingLoggerEditViewModel(NavigationService navigationService, IAuthService authService,
            IDataService dataService, ICacheService cacheService)
            : base(navigationService, authService, dataService, cacheService)
        {
        }
    }
}
