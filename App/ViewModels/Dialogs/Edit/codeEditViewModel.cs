using Core.DTOs.Cache;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Commands.EditCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal sealed class codeEditViewModel : CodingLoggerEditViewModel
    {
        public override ICommand SaveCommand { get; }
        public override ICommand CacheEditCommand { get; }
        private readonly CodingViewModel _codingViewModel;

        public codeEditViewModel(NavigationService navigationService, CodingViewModel codingViewModel, IDataService dataService,
            CodingCacheDTO codingCacheDTO, IAuthService authService, ICacheService cacheService, LOGViewModel logViewModel, int timeRemaining)
            : base(navigationService, authService, dataService, cacheService)
        {
            _codingViewModel = codingViewModel;
            Extensions.InitialiseCodingEditViewModel(_navigationService, _dataService, this, codingCacheDTO);
            SaveCommand = new SaveCommand(this, _authService, _dataService, _cacheService);
            CacheEditCommand = new EditAnnotateCommand(_navigationService, _dataService, _cacheService, this, codingViewModel, logViewModel, 
                codingCacheDTO, timeRemaining);
        }



        #region Member Functions



        public override async Task UpdateLogCountAsync()
        {
            var count = await _dataService.LogCountAsync(Category);
            LogCount = $"{_codingViewModel.CacheItems.Count} Logs Cached";
            _codingViewModel.LogCount = $"{_codingViewModel.CacheItems.Count} coding logs cached | {count} total logs";
        }



        #endregion

    }
}
