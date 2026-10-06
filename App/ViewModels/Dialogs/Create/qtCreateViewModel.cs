using Core.Enums;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.CacheCommands;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    internal sealed class qtCreateViewModel : CodingLoggerCreateViewModel
    {
        public override LogContext Context => LogContext.Qt;
        private readonly CodingQtViewModel _codingQtViewModel;
        public override bool AppFieldEnabled => false;
        public override ICommand SaveCommand { get; }
        public override ICommand AnnotateCommand { get; }

        public qtCreateViewModel(NavigationService navigationService, CodingQtViewModel codingQtViewModel, IAuthService authService, 
            IDataService dataService, ICacheService cacheService) 
            : base(navigationService, authService, dataService, cacheService)
        {
            _codingQtViewModel = codingQtViewModel;

            ApplicationName = Constants.Qt;
            ApplicationToolTip = "This field cannot be changed. Only Qt logs are allowed in this tab.";
            SaveCommand = new SaveCommand(this, _authService, _dataService, _cacheService);
            AnnotateCommand = new AnnotateCommand(_navigationService, this, _codingQtViewModel, _authService, _dataService,
            _cacheService, Context);
        }


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


        #region Log Count

        public override async Task UpdateLogCountAsync()
        {
            LogCount = await Extensions.GetQtLogCountAsync(_dataService, _codingQtViewModel);
        }

        #endregion
    }
}
