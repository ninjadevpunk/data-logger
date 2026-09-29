using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    internal abstract class LogCacheViewModel : ViewModelBase
    {
        protected readonly IDataService _dataService;
        protected readonly NavigationService _navigationService;
        public abstract LogContext Context { get; } 

        public abstract ICommand CreateLogCommand { get; }
        public abstract ICommand ReportLogCommand { get; }

        public LogCacheViewModel(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

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
        private Visibility NoLogsMessageVisibility
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

        private ObservableCollection<LOGViewModel> cacheItems = [];
        public ObservableCollection<LOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;
                NoLogsMessageVisibility = CacheItems.Count == 0 ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public bool CreateLogButtonEnabled { get; set; } = false;
        public bool ReportButtonEnabled { get; set; } = false;





        public abstract Task UpdateLogCount();


    }
}
