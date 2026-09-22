using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class CodingQtViewModel : LogCacheViewModel
    {


        public CodingQtViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<QtLOGViewModel>();
        }

        public CodingQtViewModel(string logCount, NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<QtLOGViewModel>();
            LogCount = logCount;
        }


        private ObservableCollection<QtLOGViewModel> cacheItems;
        public ObservableCollection<QtLOGViewModel> CacheItems
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

        public async Task AutoStartAsync()
        {
            await UpdateLogCount();
        }

        public async Task UpdateLogCount()
        {
            var count = await _dataService.QtLogCount();
            LogCount = $"{CacheItems.Count} qt logs cached | {count} total logs";
        }

    }
}
