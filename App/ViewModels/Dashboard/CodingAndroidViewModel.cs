using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;


namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class CodingAndroidViewModel : CodingViewModel
    {

        public CodingAndroidViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<AndroidLOGViewModel>();

        }

        public CodingAndroidViewModel(string logCount, NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<AndroidLOGViewModel>();

            LogCount = logCount;

        }

        private ObservableCollection<AndroidLOGViewModel> cacheItems;
        public ObservableCollection<AndroidLOGViewModel> CacheItems
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

        public async Task AutoStart()
        {
            if (CacheItems is not null)
                await UpdateLogCount();
        }

        public override async Task UpdateLogCount()
        {
            if (CacheItems is not null)
            {
                var count = await _dataService.ASLogCount();
                LogCount = $"{CacheItems.Count} android logs cached | {count} total logs";
            }
        }

    }
}
