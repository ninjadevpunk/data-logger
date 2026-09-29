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
    internal class QTcodeCreateViewModel : codeCreateViewModel
    {
        private readonly CodingQtViewModel _qtviewModel;
        protected override bool AppFieldEnabled => false;
        public override ICommand AnnotateCommand => new AnnotateCommand(_navigationService, this, _qtviewModel, _authService, _dataService,
            _cacheService, Context);
        public override ICommand SaveCommand => new SaveCommand(this, _authService, _dataService, _cacheService);

        public QTcodeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IAuthService authService, 
            IDataService dataService, ICacheService cacheService) 
            : base(navigationService, logCacheViewModel, authService, dataService, cacheService)
        {
            if (_codingViewModel == null)
                throw new InvalidOperationException("_codingViewModel is null in AScodeCreateViewModel constructor!");

            _qtviewModel = (CodingQtViewModel)_codingViewModel;

            ApplicationName = Constants.Qt;
            ApplicationToolTip = "This field cannot be changed. Only Android Studio logs are allowed in this tab.";
            QtOnly = true;

        }


        public override async Task AutoStartAsync(bool qtOnly)
        {
            await _dataService.InitialiseProjectsLISTAsync(Category);
            await LoadQtProjectsAndApplicationsAsync();
            await LoadQtOutputsAndTypes();
            await UpdateLogCount();
        }

        private async Task LoadQtProjectsAndApplicationsAsync()
        {
            _projects.Clear();
            _applications.Clear();

            var items = await _dataService.InitialiseProjectsLISTAsync(Constants.Qt);

            foreach (var project in items)
            {
                _projects.Add(project);
            }
        }

        private async Task LoadQtOutputsAndTypes()
        {
            Output = "Console Application";
            Type = "Build";
            _outputs.Clear();
            _types.Clear();

            foreach(var output in await _dataService.ListQtOutputsAsync())
            {
                _outputs.Add(output);
            }

            foreach(var type in await _dataService.ListQtTypesAsync())
            {
                _types.Add(type);
            }
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
