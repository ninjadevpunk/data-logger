using Core.DTOs.Cache;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Commands.EditCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using FileSys.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal sealed class codeEditViewModel : CodingLoggerEditViewModel
    {
        public override ICommand SaveCommand { get; }
        public override ICommand CacheEditCommand { get; }
        private readonly CodingViewModel _codingViewModel;

        public codeEditViewModel(NavigationService navigationService, CodingViewModel codingViewModel, IDataService dataService,
            CodingCacheDTO codingCacheDTO, IAuthService authService, ICacheService cacheService)
            : base(navigationService, authService, dataService, cacheService)
        {
            _codingViewModel = codingViewModel;
            ProjectName = codingCacheDTO.ProjectName;
            ApplicationName = codingCacheDTO.ApplicationName;

            StartDate = codingCacheDTO.Start;
            StartHours = StartDate.Hour;
            StartMinutes = StartDate.Minute;
            StartSeconds = StartDate.Second;
            StartMilliseconds = StartDate.Millisecond;

            EndDate = codingCacheDTO.End;
            EndHours = EndDate.Hour;
            EndMinutes = EndDate.Minute;
            EndSeconds = EndDate.Second;
            EndMilliseconds = EndDate.Millisecond;

            Output = codingCacheDTO.OutputName;
            Type = codingCacheDTO.TypeName;
            PostIts = new ObservableCollection<StickyPostItViewModel>(codingCacheDTO.PostIts.Select(p =>
            {
                var postIt = (PostItCacheDTO)p;
                return new CacheStickyPostItViewModel(_navigationService, _dataService, this, postIt);
            }));
            BugsFound = codingCacheDTO.Bugs;
            ApplicationOpened = codingCacheDTO.Success;

            SaveCommand = new SaveCommand(this, _authService, _dataService, _cacheService);
            CacheEditCommand = new EditAnnotateCommand(_navigationService, _dataService, this, codingCacheDTO);

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
