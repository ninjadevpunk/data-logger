using Core.Enums;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class ViewerOKCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogContext _cacheContext = LogContext.Coding;
        private ViewType _viewType = ViewType.Cache;




        public ViewerOKCommand(NavigationService navigationService, LogContext logContext)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _cacheContext = logContext;
        }


        public ViewerOKCommand(NavigationService navigationService, ViewType viewType)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _viewType = viewType;
        }

        public ViewerOKCommand(NavigationService navigationService, LogContext logContext, ViewType viewType)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _cacheContext = logContext;
            _viewType = viewType;
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
