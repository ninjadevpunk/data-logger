using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class ViewerOKCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly CacheContext _cacheContext = CacheContext.Coding;
        private ViewType _viewType = ViewType.Cache;




        public ViewerOKCommand(NavigationService navigationService, CacheContext cacheContext)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
            }
            catch (Exception)
            {
                // TODO
            }
        }


        public ViewerOKCommand(NavigationService navigationService, ViewType viewType)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewType = viewType;
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public ViewerOKCommand(NavigationService navigationService, CacheContext cacheContext, ViewType viewType)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
                _viewType = viewType;
            }
            catch (Exception)
            {
                // TODO
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if(_viewType == ViewType.Cache)
                await _navigationService.NavigateToLogCachePage(_cacheContext);
            else
            {
                _navigationService.GoBack(false);
            }
        }
    }
}
