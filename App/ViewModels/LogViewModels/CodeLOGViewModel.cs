using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    /// <summary>
    /// LOG ViewModel class for a log cached in the log cache. Specialised for coding logs.
    /// </summary>
    internal sealed class CodeLOGViewModel : LOGViewModel
    {
        public CodeLOGViewModel(NavigationService navigationService, CodingCacheDTO codingCacheDTO, CodingViewModel codingViewModel,
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true)
            : base(navigationService, codingCacheDTO, codingViewModel, dataService, cacheService, isCacheSave)
        {
            EditCommand = new EditCommand(navigationService, codingCacheDTO, _timer);
            ViewCommand = new ViewCommand(_navigationService, codingCacheDTO);

            if (_isCacheSave)
                _cacheMaster.SaveCodeCache(codingCacheDTO);
        }

        protected override async void AutoDeleteLogCacheItem()
        {
            CleanUp();
            DeleteCacheItemCommand.Execute(this);
        }
    }
}
