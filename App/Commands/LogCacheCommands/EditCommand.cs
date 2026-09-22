using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class EditCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogCacheViewModel _dashboard;
        private readonly CacheContext _cacheContext;

        public EditCommand(CacheContext cacheContext, NavigationService navigationService, LogCacheViewModel logCacheViewModel)
        {
            try
            {
                _cacheContext = cacheContext;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");

                // TODO
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                switch (_cacheContext)
                {
                    case CacheContext.Qt:
                        {
                            var log = parameter as QtLOGViewModel;
                            

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            var log = parameter as AndroidLOGViewModel;

                            break;
                        }
                    case CacheContext.Coding:
                        {
                            var log = parameter as CodeLOGViewModel;

                            await _navigationService.NavigateToLoggerEditor(log);

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            var log = parameter as GraphicsLOGViewModel;

                            break;
                        }
                    case CacheContext.Film:
                        {
                            var log = parameter as FilmLOGViewModel;

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            var log = parameter as FlexiLOGViewModel;

                            break;
                        }
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
