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
            await LoadProjects();
            await LoadApplications();
            await LoadOutputs();
            await LoadTypes();
            await UpdateLogCount();
        }

        protected override async Task LoadProjects()
        {
            Projects = (await _dataService.InitialiseProjectsLISTAsync(Category)).ToHashSet();
        }

        protected override async Task LoadApplications()
        {
            ApplicationName = Constants.VisualStudio;
            Applications = (await _dataService.InitialiseApplicationsLISTAsync(Category)).ToHashSet();
        }

        protected override async Task LoadOutputs()
        {
            Output = "C# Application (*.exe)";
            Outputs = (await _dataService.ListOutputsAsync(LOG.CATEGORY.CODING)).ToHashSet();
        }

        protected override async Task LoadTypes()
        {
            Type = "Exception";
            Types = (await _dataService.ListTypesAsync(LOG.CATEGORY.CODING)).ToHashSet();
        }
    }
}
