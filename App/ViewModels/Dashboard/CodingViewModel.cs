using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class CodingViewModel : LogCacheViewModel
    {
        private ObservableCollection<CodeLOGViewModel> cacheItems;
        public ObservableCollection<CodeLOGViewModel> CacheItems
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


        public CodingViewModel(NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();
        }

        public CodingViewModel(string logCount, NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            LogCount = logCount;
        }

        public async Task AutoStartAsync()
        {
            await UpdateLogCount();
        }

        public void OnLogOut()
        {
            CacheItems.Clear();
            LogCount = "";
        }

        public virtual async Task UpdateLogCount()
        {
            var count = await _dataService.LogCount(LOG.CATEGORY.CODING);
            LogCount = $"{CacheItems.Count} coding logs cached | {count} total logs";
        }
    }
}
