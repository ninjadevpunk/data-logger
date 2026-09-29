using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    internal class LOGViewModelFactory
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;

        public LOGViewModelFactory(NavigationService navigationService, IDataService dataService, ICacheService cacheService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "QtLOGViewModelFactory");
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService), "QtLOGViewModelFactory");
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService), "QtLOGViewModelFactory");
        }

        public QtLOGViewModel Create(QtCacheDTO qtCacheDto, CodingQtViewModel codingQtViewModel, bool isCacheSave = true)
        {
            return new QtLOGViewModel(_navigationService, qtCacheDto, codingQtViewModel, _dataService, _cacheService, isCacheSave);
        }

        public AndroidLOGViewModel Create(AndroidCacheDTO androidCacheDTO, CodingAndroidViewModel codingAndroidViewModel, bool isCacheSave = true)
        {
            return new AndroidLOGViewModel(_navigationService, androidCacheDTO, codingAndroidViewModel, _dataService, _cacheService, isCacheSave);
        }

        public CodeLOGViewModel Create(CodingCacheDTO codingCacheDTO, CodingViewModel codingViewModel, bool isCacheSave = true)
        {
            return new CodeLOGViewModel(_navigationService, codingCacheDTO, codingViewModel, _dataService, _cacheService, isCacheSave);
        }
    }
}
