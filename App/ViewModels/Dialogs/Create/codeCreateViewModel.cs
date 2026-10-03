using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.CacheCommands;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    internal sealed class codeCreateViewModel : CodingLoggerCreateViewModel
    {
        private readonly CodingViewModel _codingViewModel;

        public override ICommand SaveCommand { get; }
        public override ICommand AnnotateCommand { get; }

        public codeCreateViewModel(NavigationService navigationService, CodingViewModel codingViewModel, IAuthService authService,
            IDataService dataService, ICacheService cacheService)
            : base(navigationService, authService, dataService, cacheService)
        {
            _codingViewModel = codingViewModel;

            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
            SaveCommand = new SaveCommand(this, _authService, _dataService, _cacheService);
            AnnotateCommand = new AnnotateCommand(_navigationService, this, _codingViewModel, _authService, _dataService,
            _cacheService, Context);
        }





        #region Member Functions

        public override async Task UpdateLogCount()
        {
            var count = await _dataService.LogCountAsync(Category);
            LogCount = $"{_codingViewModel.CacheItems.Count} Logs Cached";
            _codingViewModel.LogCount = $"{_codingViewModel.CacheItems.Count} coding logs cached | {count} total logs";
        }

        #endregion


    }
}
