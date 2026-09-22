using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteCacheItemCommand : AsyncCommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly IDataService _dataService;
        private readonly bool _timeUp;

        public DeleteCacheItemCommand(LogCacheViewModel logCacheViewModel, IDataService dataService, bool timeIsUp)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _timeUp = timeIsUp;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Argument null exception found: {ex.Message}");
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
                var logViewModel = parameter as LOGViewModel;

                if (logViewModel == null)
                    throw new NullReferenceException("logViewModel is null.");

                switch (logViewModel.LOGViewModelContext)
                {
                    case CacheContext.Qt:
                        await HandleQtCacheItem((QtLOGViewModel)logViewModel);
                        break;
                    case CacheContext.AndroidStudio:
                        await HandleAndroidCacheItem((AndroidLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Coding:
                        await HandleCodingCacheItem((CodeLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Graphics:
                        await HandleGraphicsCacheItem((GraphicsLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Film:
                        await HandleFilmCacheItem((FilmLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Flexi:
                        await HandleFlexiCacheItem((FlexiLOGViewModel)logViewModel);
                        break;
                }
            }
            catch (InvalidCastException castx)
            {
                await _dataService.HandleExceptionAsync(castx, "Execute() DeleteCacheItemCommand", "InvalidCastException");
            }
            catch (Exception e)
            {
                await _dataService.HandleExceptionAsync(e, "Execute() DeleteCacheItemCommand");
            }
        }

        private async Task HandleQtCacheItem(QtLOGViewModel item)
        {
            var tempQtDashBoardItems = ((CodingQtViewModel)_viewModel).CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                bool isLogged = await _dataService.CreateLOG(item._QtcodingLOG);

                if (isLogged)
                {
                    tempQtDashBoardItems.Remove(item);
                    ((CodingQtViewModel)_viewModel).CacheItems = tempQtDashBoardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                await item._timer.DisposeAsync();
                tempQtDashBoardItems.Remove(item);
                ((CodingQtViewModel)_viewModel).CacheItems = tempQtDashBoardItems;
            }
        }

        private async Task HandleAndroidCacheItem(AndroidLOGViewModel item)
        {
            var tempASDashboardItems = ((CodingAndroidViewModel)_viewModel).CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                bool isLogged = await _dataService.CreateLOG(item._AndroidCodingLOG);

                if (isLogged)
                {
                    tempASDashboardItems.Remove(item);
                    ((CodingAndroidViewModel)_viewModel).CacheItems = tempASDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                await item._timer.DisposeAsync();
                tempASDashboardItems.Remove(item);
                ((CodingAndroidViewModel)_viewModel).CacheItems = tempASDashboardItems;
            }
        }

        private async Task HandleCodingCacheItem(CodeLOGViewModel item)
        {
            var tempCodingDashboardItems = ((CodingViewModel)_viewModel).CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                bool isLogged = await _dataService.CreateLOG(item._CodeLOG);

                if (isLogged)
                {
                    tempCodingDashboardItems.Remove(item);
                    ((CodingViewModel)_viewModel).CacheItems = tempCodingDashboardItems;
                    await ((CodingViewModel)_viewModel).UpdateLogCount();

                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                await item._timer.DisposeAsync();
                tempCodingDashboardItems.Remove(item);
                ((CodingViewModel)_viewModel).CacheItems = tempCodingDashboardItems;
                await ((CodingViewModel)_viewModel).UpdateLogCount();
            }
        }

        private async Task HandleGraphicsCacheItem(GraphicsLOGViewModel item)
        {
            var tempGraphicsDashboard = ((GraphicsViewModel)_viewModel).CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                bool isLogged = await _dataService.CreateLOG(item._GraphicsLOG);

                if (isLogged)
                {
                    tempGraphicsDashboard.Remove(item);
                    ((GraphicsViewModel)_viewModel).CacheItems = tempGraphicsDashboard;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempGraphicsDashboard.Remove(item);
                ((GraphicsViewModel)_viewModel).CacheItems = tempGraphicsDashboard;
            }
        }

        private async Task HandleFilmCacheItem(FilmLOGViewModel item)
        {
            var tempFilmDashboardItems = ((FilmViewModel)_viewModel).CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                bool isLogged = await _dataService.CreateLOG(item._FilmLOG);

                if (isLogged)
                {
                    tempFilmDashboardItems.Remove(item);
                    ((FilmViewModel)_viewModel).CacheItems = tempFilmDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempFilmDashboardItems.Remove(item);
                ((FilmViewModel)_viewModel).CacheItems = tempFilmDashboardItems;
            }
        }

        private async Task HandleFlexiCacheItem(FlexiLOGViewModel item)
        {
            var tempFlexiDashboardItems = ((FlexiViewModel)_viewModel).CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                bool isLogged = await _dataService.CreateLOG(item._FlexiLOG);

                if (isLogged)
                {
                    tempFlexiDashboardItems.Remove(item);
                    ((FlexiViewModel)_viewModel).CacheItems = tempFlexiDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempFlexiDashboardItems.Remove(item);
                ((FlexiViewModel)_viewModel).CacheItems = tempFlexiDashboardItems;
            }
        }









    }
}
