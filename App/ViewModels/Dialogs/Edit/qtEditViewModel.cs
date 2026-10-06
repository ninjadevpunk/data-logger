using Core.DTOs.Cache;
using Core.Interfaces;
using Core.Utilities;
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
    internal sealed class qtEditViewModel : CodingLoggerEditViewModel
    {
        private readonly CodingQtViewModel _codingQtViewModel;
        public override bool AppFieldEnabled => false;

        public override ICommand CacheEditCommand { get; }
        public override ICommand SaveCommand { get; }


        public qtEditViewModel(NavigationService navigationService, CodingQtViewModel codingQtViewModel, IAuthService authService, IDataService dataService, 
            ICacheService cacheService, QtCacheDTO qtCacheDTO, LOGViewModel logViewModel, int timeRemaining) 
            : base(navigationService, authService, dataService, cacheService)
        {
            _codingQtViewModel = codingQtViewModel;
            Extensions.InitialiseCodingEditViewModel(_navigationService, _dataService, this, qtCacheDTO);

            ApplicationToolTip = "This field cannot be changed. Only Qt logs are allowed in this tab.";

            SaveCommand = new SaveCommand(this, _authService, _dataService, _cacheService);
            CacheEditCommand = new EditAnnotateCommand(_navigationService, _dataService, _cacheService, this, codingQtViewModel, logViewModel, 
                qtCacheDTO, timeRemaining);
        }



        #region Initialization Methods



        protected override async Task LoadProjectsAsync() => Projects = [.. (await _dataService.InitialiseProjectsLISTAsync(Constants.Qt))];

        protected override async Task LoadApplicationsAsync() => Applications = [];

        protected override async Task LoadOutputsAsync()
        {
            Output = "QtQuick Application";
            Outputs = [.. (await _dataService.ListQtOutputsAsync())];
        }


        protected override async Task LoadTypesAsync()
        {
            Type = "Build";
            Types = [.. (await _dataService.ListQtTypesAsync())];
        }




        #endregion




        public override async Task UpdateLogCountAsync()
        {
            LogCount = await Extensions.GetQtLogCountAsync(_dataService, _codingQtViewModel);
        }
    }
}
