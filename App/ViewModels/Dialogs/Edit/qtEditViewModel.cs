using Core.DTOs.Cache;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal class qtEditViewModel : CodingLoggerEditViewModel
    {
        private readonly CodingQtViewModel _codingQtViewModel;
        private readonly QtCacheDTO _qtCacheDTO;
        public override bool AppFieldEnabled => false;

        public override ICommand CacheEditCommand => throw new NotImplementedException();
        public override ICommand SaveCommand => throw new NotImplementedException();


        public qtEditViewModel(NavigationService navigationService, CodingQtViewModel codingQtViewModel, IAuthService authService, IDataService dataService, 
            ICacheService cacheService, QtCacheDTO qtCacheDTO) 
            : base(navigationService, authService, dataService, cacheService)
        {
            _codingQtViewModel = codingQtViewModel;
            _qtCacheDTO = qtCacheDTO;
        }


        public override Task UpdateLogCount()
        {
            throw new NotImplementedException();
        }
    }
}
