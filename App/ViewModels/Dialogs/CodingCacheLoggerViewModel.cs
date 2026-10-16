using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    /// <summary>
    /// Represents a base view model for coding cache logger dialogs, providing common functionality for creating and
    ///  editing logs in the cache.
    /// </summary>
    internal abstract class CodingCacheLoggerViewModel : CodingLoggerViewModel
    {
        protected readonly ICacheService _cacheService;

        public override ICommand AddPostItCommand => new AddPostItCommand(_navigationService, this);

        public CodingCacheLoggerViewModel(NavigationService navigationService, IAuthService authService, 
            IDataService dataService, ICacheService cacheService) 
            : base(navigationService, authService, dataService)
        {
            _cacheService = cacheService;
        }
    }
}
