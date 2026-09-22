using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class GraphicsViewModel : LogCacheViewModel
    {


        public GraphicsViewModel(NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<GraphicsLOGViewModel>();
        }


        public GraphicsViewModel(string logCount, NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<GraphicsLOGViewModel>();

            LogCount = logCount;

        }

        private ObservableCollection<GraphicsLOGViewModel> cacheItems;
        public ObservableCollection<GraphicsLOGViewModel> CacheItems
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
            var count = await _dataService.LogCount(Models.LOG.CATEGORY.GRAPHICS);
            LogCount = $"{CacheItems.Count} graphics logs cached | {count} total logs";
        }

    }
}
