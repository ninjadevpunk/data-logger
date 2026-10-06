using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    internal abstract class CodingLoggerViewModel : LoggerViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override LogContext Context => LogContext.Coding;
        public override string LogType => "CODING LOG";

        /// <summary>
        /// Adds a PostIt to the PostIt list.
        /// </summary>
        public abstract ICommand AddPostItCommand { get; }


        protected CodingLoggerViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService) 
            : base(navigationService, authService, dataService)
        {
        }

        


        private int bugsFound;
        public int BugsFound
        {
            get => bugsFound;
            set
            {
                bugsFound = value;
                OnPropertyChanged(nameof(BugsFound));
            }
        }

        private bool applicationOpened;
        public bool ApplicationOpened
        {
            get => applicationOpened;
            set
            {
                applicationOpened = value;
                OnPropertyChanged(nameof(ApplicationOpened));
            }
        }


        internal override async Task AutoStartAsync()
        {
            await LoadProjectsAsync();
            await LoadApplicationsAsync();
            await LoadOutputsAsync();
            await LoadTypesAsync();
            await UpdateLogCountAsync();
        }

        protected override async Task LoadProjectsAsync()
        {
            Projects = [.. (await _dataService.InitialiseProjectsLISTAsync(Category))];
        }

        protected override async Task LoadApplicationsAsync()
        {
            ApplicationName = Constants.VisualStudio;
            Applications = [.. (await _dataService.InitialiseApplicationsLISTAsync(Category))];
        }

        protected override async Task LoadOutputsAsync()
        {
            Output = "C# Application (*.exe)";
            Outputs = [.. (await _dataService.ListOutputsAsync(LOG.CATEGORY.CODING))];
        }

        protected override async Task LoadTypesAsync()
        {
            Type = "Exception";
            Types = [.. (await _dataService.ListTypesAsync(LOG.CATEGORY.CODING))];
        }
    }
}
