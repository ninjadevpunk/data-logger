using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class LogCacheViewModel : ViewModelBase
    {
        protected readonly IDataService _dataService;
        public NavigationService _navigationService;

        public LogCacheViewModel(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            CreateLogCommand = new CreateLogCommand(_navigationService);
            ReportLogCommand = new ReportCommand(_navigationService);
        }

        private string logCount = "0 logs cached | 0 total logs";
        public string LogCount
        {
            get
            {
                return logCount;
            }
            set
            {
                logCount = value;
                OnPropertyChanged(nameof(LogCount));
            }
        }


        private Visibility noLogsMessageVisibility;
        public Visibility NoLogsMessageVisibility
        {
            get
            {
                return noLogsMessageVisibility;
            }
            set
            {
                noLogsMessageVisibility = value;
                OnPropertyChanged(nameof(NoLogsMessageVisibility));
            }
        }


        public bool CreateLogButtonEnabled { get; set; } = false;
        public bool ReportButtonEnabled { get; set; } = false;

        public ICommand CreateLogCommand { get; set; }
        public ICommand ReportLogCommand { get; set; }



    }
}
