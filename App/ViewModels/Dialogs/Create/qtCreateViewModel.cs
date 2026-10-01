using Core.Enums;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.CacheCommands;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    internal sealed class qtCreateViewModel : CodingLoggerCreateViewModel
    {
        public override LogContext Context => LogContext.Qt;
        private readonly CodingQtViewModel _qtviewModel;
        public override bool AppFieldEnabled => false;
        public override ICommand SaveCommand { get; }
        public override ICommand AnnotateCommand { get; }

        public qtCreateViewModel(NavigationService navigationService, CodingQtViewModel codingQtViewModel, IAuthService authService, 
            IDataService dataService, ICacheService cacheService) 
            : base(navigationService, authService, dataService, cacheService)
        {
            _qtviewModel = codingQtViewModel;

            ApplicationName = Constants.Qt;
            ApplicationToolTip = "This field cannot be changed. Only Qt logs are allowed in this tab.";
            SaveCommand = new SaveCommand(this, _authService, _dataService, _cacheService);
            AnnotateCommand = new AnnotateCommand(_navigationService, this, _qtviewModel, _authService, _dataService,
            _cacheService, Context);
        }


        protected override async Task LoadProjects() => Projects = (await _dataService.InitialiseProjectsLISTAsync(Constants.Qt)).ToHashSet();

        protected override async Task LoadApplications() => Applications = [];

        protected override async Task LoadOutputs()
        {
            Output = "QtQuick Application";
            Outputs = (await _dataService.ListQtOutputsAsync()).ToHashSet();
        }


        protected override async Task LoadTypes()
        {
            Type = "Build";
            Types = (await _dataService.ListQtTypesAsync()).ToHashSet();
        }


        #region Log Count

        public override async Task UpdateLogCount()
        {
            var count = await _dataService.QtLogCountAsync();
            LogCount = $"{_qtviewModel.CacheItems.Count} Logs Cached";
            _qtviewModel.LogCount = $"{_qtviewModel.CacheItems.Count} qt logs cached | {count} total logs";
        }

        #endregion
    }
}
