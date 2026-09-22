using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class FlexiViewModel : LogCacheViewModel
    {


        public FlexiViewModel(NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<FlexiLOGViewModel>();

            _navigationService = navigationService;
        }


        public FlexiViewModel(string logCount, NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<FlexiLOGViewModel>();

            _navigationService = navigationService;

            LogCount = logCount;
        }


        private ObservableCollection<FlexiLOGViewModel> cacheItems;
        public ObservableCollection<FlexiLOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;

                NoLogsMessageVisibility = CacheItems.Count == 0 ? Visibility.Visible : Visibility.Hidden;
                UpdateLogCount();

                OnPropertyChanged(nameof(CacheItems));
            }
        }

        public async void UpdateLogCount()
        {
            var count = await _dataService.LogCount(LOG.CATEGORY.NOTES);
            LogCount = $"{CacheItems.Count} flexible logs cached | {count} total logs";
        }

    }
}
