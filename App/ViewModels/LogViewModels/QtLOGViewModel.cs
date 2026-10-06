using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data.Interfaces;
using FileSys.Interfaces;
using Data_Logger_1._3.Services;
using Core.DTOs.Cache;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    /// <summary>
    /// LOG ViewModel class for a log cached in the log cache. Specialised for Qt logs.
    /// </summary>
    internal sealed class QtLOGViewModel : LOGViewModel
    {
        public QtLOGViewModel(NavigationService navigationService, QtCacheDTO qtCodingCacheDTO, CodingQtViewModel codingQtViewModel,
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true) :
            base(navigationService, qtCodingCacheDTO, codingQtViewModel, dataService, cacheService, isCacheSave)
        {
            EditCommand = new EditCommand(navigationService, qtCodingCacheDTO, _timer);
            ViewCommand = new ViewCommand(navigationService, qtCodingCacheDTO);

            if (_isCacheSave)
                _cacheMaster.SaveQtCodeCache(qtCodingCacheDTO);
        }


        protected override async void DeleteCacheItem()
        {
            base.DeleteCacheItem();
            DeleteCacheItemCommand.Execute(this);
        }
    }
}
