using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    internal sealed class CodeLOGViewModel : LOGViewModel
    {
        private readonly CodingCacheDTO _codingCacheDTO;




        #region Constructors



        public CodeLOGViewModel(NavigationService navigationService, CodingCacheDTO codingCacheDTO, CodingViewModel codingViewModel, 
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true)
            : base(navigationService, codingCacheDTO, codingViewModel, dataService, cacheService, isCacheSave)
        {
            _codingCacheDTO = codingCacheDTO;

            EditCommand = new EditCommand(LOGViewModelContext, navigationService, _codingCacheDTO);
            ViewCommand = new ViewCommand(_navigationService, codingCacheDTO);
            

            if(_isCacheSave)
            {
                _cacheMaster.SaveCodeCache(codingCacheDTO);
            }

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
