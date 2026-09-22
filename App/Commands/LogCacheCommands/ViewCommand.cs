using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

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
        private readonly CacheContext _cacheContext = CacheContext.Coding;
        private ViewType viewType = ViewType.Cache;
        private readonly LOG _log;

        public ViewCommand(NavigationService navigationService, CacheContext cacheContext)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
                viewType = ViewType.Cache;
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }

        public ViewCommand(NavigationService navigationService, CacheContext cacheContext, ViewType viewType,
            CodingLOG codingLog)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _log = codingLog;
                _cacheContext = cacheContext;
                this.viewType = viewType;
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }

        public ViewCommand(LOG log, NavigationService navigationService, CacheContext cacheContext, ViewType viewType)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
                this.viewType = viewType;
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
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
                                case CacheContext.Qt:
                                    {
                                        var log = parameter as QtLOGViewModel;

                                        if(log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.AndroidStudio:
                                    {
                                        var log = parameter as AndroidLOGViewModel;

                                        if (log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Coding:
                                    {
                                        var log = parameter as CodeLOGViewModel;

                                        if (log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Graphics:
                                    {
                                        var log = parameter as GraphicsLOGViewModel;

                                        if (log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Film:
                                    {
                                        var log = parameter as FilmLOGViewModel;

                                        if (log == null)
                                            throw new ArgumentNullException(nameof(log));

                                        await _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Flexi:
                                    {
                                        var log = parameter as FlexiLOGViewModel;

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
