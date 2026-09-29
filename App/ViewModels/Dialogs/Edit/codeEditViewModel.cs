using Core.DTOs.Cache;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal class codeEditViewModel : LoggerEditViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override LogContext Context => LogContext.Coding;
        public override string LogType => "CODING LOG";
        public override ICommand SaveCommand => new SaveCommand(this, _authService, _dataService, _cacheService);

        protected readonly CodingViewModel _codingViewModel;

        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService,
            CodingCacheDTO codingCacheDTO, CodeLOGViewModel codeLogViewModel, IAuthService authService, ICacheService cacheService)
            : base(navigationService, logCacheViewModel, authService, dataService, cacheService)
        {
            _codingViewModel = (CodingViewModel)logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel),
                    "codeCreateViewModel");
            ProjectName = codingCacheDTO.ProjectName;
            ApplicationName = codingCacheDTO.ApplicationName;

            StartDate = codingCacheDTO.Start;
            StartHours = codingCacheDTO.Start.Hour;
            StartMinutes = codingCacheDTO.Start.Minute;
            StartSeconds = codingCacheDTO.Start.Second;
            StartMilliseconds = codingCacheDTO.Start.Millisecond;

            EndDate = codingCacheDTO.End;
            EndHours = codingCacheDTO.End.Hour;
            EndMinutes = codingCacheDTO.End.Minute;
            EndSeconds = codingCacheDTO.End.Second;
            EndMilliseconds = codingCacheDTO.End.Millisecond;


            //EditCommand = new EditAnnotateCommand(abstract, codeLogViewModel);

        }



        #region Member Functions

        public override async Task UpdateLogCount()
        {
            if (_codingViewModel != null)
            {
                var count = await _dataService.LogCountAsync(Category);
                LogCount = $"{_codingViewModel.CacheItems.Count} Logs Cached";
                _codingViewModel.LogCount = $"{_codingViewModel.CacheItems.Count} coding logs cached | {count} total logs";
            }
        }

        #endregion



    }
}
