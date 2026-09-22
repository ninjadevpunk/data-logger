using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class FilmViewModel : LogCacheViewModel
    {


        public FilmViewModel(NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<FilmLOGViewModel>();
        }

        public FilmViewModel(string logCount, NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<FilmLOGViewModel>();

            LogCount = logCount;
        }

        private ObservableCollection<FilmLOGViewModel> cacheItems;
        public ObservableCollection<FilmLOGViewModel> CacheItems
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
            var count = await _dataService.LogCount(LOG.CATEGORY.FILM);
            LogCount = $"{CacheItems.Count} film logs cached | {count} total logs";
        }

    }
}
