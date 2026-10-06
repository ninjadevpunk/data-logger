using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    /// <summary>
    /// LOG ViewModel class for a log cached in the log cache. Specialised for Android logs.
    /// </summary>
    internal sealed class AndroidLOGViewModel : LOGViewModel
    {
        public AndroidLOGViewModel(NavigationService navigationService, AndroidCacheDTO androidCacheDTO, CodingAndroidViewModel codingAndroidViewModel, 
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true) 
            : base(navigationService, androidCacheDTO, codingAndroidViewModel, dataService, cacheService, isCacheSave)
        {
            EditCommand = new EditCommand(LogContext.AndroidStudio, navigationService, androidCacheDTO);
            ViewCommand = new ViewCommand(_navigationService, androidCacheDTO);

            if (_isCacheSave)
                _cacheMaster.SaveAndroidCodeCache(androidCacheDTO);
        }


        protected override async void DeleteCacheItem()
        {
            base.DeleteCacheItem();
            DeleteCacheItemCommand.Execute(this);
        }
    }
}
