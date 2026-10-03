using Core.DTOs.Cache;
using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    internal sealed class AndroidLOGViewModel : CodeLOGViewModel
    {

        #region Constructors



        public AndroidLOGViewModel(NavigationService navigationService, AndroidCacheDTO androidCacheDTO, CodingAndroidViewModel codingAndroidViewModel, 
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true) 
            : base(navigationService, androidCacheDTO, codingAndroidViewModel, dataService, cacheService, isCacheSave)
        {
            EditCommand = new EditCommand(LogContext.AndroidStudio, navigationService, androidCacheDTO);
            ViewCommand = new ViewCommand(_navigationService, androidCacheDTO);

            if(_isCacheSave)
                _cacheMaster.SaveAndroidCodeCache(androidCacheDTO);
        }



        #endregion







        #region Member Functions




        protected override void DeleteCacheItem()
        {
            DeleteCacheFile(StartAsCacheID, LOGViewModelContext);
            DeleteCacheItemCommand.Execute(this);
        }





        #endregion
    }
}
