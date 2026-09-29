using Core.DTOs.Cache;
using Core.Enums;
using Core.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public enum ViewType
    {
        Cache,
        Log
    }

    public class ViewCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogContext _cacheContext = LogContext.Coding;
        private ViewType viewType = ViewType.Cache;
        private readonly LogCacheDTO _logCacheDTO;

        public ViewCommand(NavigationService navigationService, LogContext logContext)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _cacheContext = logContext;
            viewType = ViewType.Cache;
        }

        public ViewCommand(NavigationService navigationService, LogContext logContext, ViewType viewType,
            LogCacheDTO logCacheDTO)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _logCacheDTO = logCacheDTO;
            _cacheContext = logContext;
            this.viewType = viewType;
        }

        public ViewCommand(LOG log, NavigationService navigationService, LogContext logContext, ViewType viewType)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _cacheContext = logContext;
            this.viewType = viewType;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                switch (viewType)
                {
                    case ViewType.Cache:
                        {
                            switch (_cacheContext)
                            {
                                case LogContext.Qt:
                                    {
                                        var log = parameter as QtLOGViewModel;

                                        if(log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case LogContext.AndroidStudio:
                                    {
                                        var log = parameter as AndroidLOGViewModel;

                                        if (log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case LogContext.Coding:
                                    {
                                        var log = parameter as CodeLOGViewModel;

                                        if (log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                            }
                            break;
                        }
                    case ViewType.Log:
                        {

                            switch (_cacheContext)
                            {
                                default:
                                    {
                                        await _navigationService.NavigateToViewer(_log);

                                        break;
                                    }
                            }

                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
