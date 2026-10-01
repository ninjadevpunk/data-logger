using Core.DTOs.Cache;
using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class DeleteCacheItemCommand : AsyncCommandBase
    {
        private readonly LogCacheDTO _logCacheDTO;
        private readonly LogCacheViewModel _logCacheViewModel;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly bool _timeUp;
        private readonly Timer? _timer;

        public DeleteCacheItemCommand(LogCacheViewModel logCacheViewModel, IDataService dataService, ICacheService cacheService,
            LogCacheDTO logCacheDTO, bool timeIsUp, Timer? timer)
        {
            _logCacheViewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logCacheDTO = logCacheDTO ?? throw new ArgumentNullException(nameof(logCacheDTO));
            _timeUp = timeIsUp;
            _timer = timer;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var logViewModel = (LOGViewModel)parameter ?? throw new ArgumentNullException("logViewModel", "DeleteCacheItemCommand<TLOGViewModel>");
                await HandleCodingCacheItem(logViewModel);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }


        private async Task HandleCodingCacheItem(LOGViewModel item)
        {
            var cachedDashboardItems = _logCacheViewModel.CacheItems;
            var context = _logCacheDTO.Context;
            var startDate = DateTime.Parse(_logCacheDTO.Start);

            switch (_logCacheDTO.Context)
            {
                case LogContext.Qt when _logCacheDTO is QtCacheDTO qtCacheDTO:
                    {
                        if (_timeUp)
                        {
                            // Send data to database first
                            bool isLogged = await _dataService.CreateLOGAsync(qtCacheDTO);

                            if (isLogged)
                            {
                                cachedDashboardItems.Remove(item);
                                _logCacheViewModel.CacheItems = cachedDashboardItems;
                                await _logCacheViewModel.UpdateLogCount();

                            }
                        }
                        else
                        {

                            _cacheService.DeleteCacheFile(_logCacheDTO.UserId, startDate.ToString("yyyyMMdd_HHmmss"), context);

                            cachedDashboardItems.Remove(item);
                            _logCacheViewModel.CacheItems = cachedDashboardItems;
                            await _logCacheViewModel.UpdateLogCount();
                        }

                        break;
                    }
                case LogContext.AndroidStudio when _logCacheDTO is AndroidCacheDTO androidCacheDTO:
                    {
                        if (_timeUp)
                        {
                            // Send data to database first
                            bool isLogged = await _dataService.CreateLOGAsync(androidCacheDTO);

                            if (isLogged)
                            {
                                cachedDashboardItems.Remove(item);
                                _logCacheViewModel.CacheItems = cachedDashboardItems;
                                await _logCacheViewModel.UpdateLogCount();

                            }
                        }
                        else
                        {

                            _cacheService.DeleteCacheFile(_logCacheDTO.UserId, startDate.ToString("yyyyMMdd_HHmmss"), context);

                            cachedDashboardItems.Remove(item);
                            _logCacheViewModel.CacheItems = cachedDashboardItems;
                            await _logCacheViewModel.UpdateLogCount();
                        }

                        break;
                    }
                default:
                    {
                        if (_timeUp)
                        {
                            // Send data to database first
                            bool isLogged = await _dataService.CreateLOGAsync((CodingCacheDTO)_logCacheDTO);

                            if (isLogged)
                            {
                                cachedDashboardItems.Remove(item);
                                _logCacheViewModel.CacheItems = cachedDashboardItems;
                                await _logCacheViewModel.UpdateLogCount();

                            }
                        }
                        else
                        {

                            _cacheService.DeleteCacheFile(_logCacheDTO.UserId, startDate.ToString("yyyyMMdd_HHmmss"), context);

                            cachedDashboardItems.Remove(item);
                            _logCacheViewModel.CacheItems = cachedDashboardItems;
                            await _logCacheViewModel.UpdateLogCount();
                        }

                        break;
                    }
            }
        }



    }
}
