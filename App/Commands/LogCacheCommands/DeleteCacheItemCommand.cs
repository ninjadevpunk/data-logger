using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class DeleteCacheItemCommand<TLOGViewModel> : AsyncCommandBase
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
                var logViewModel = (TLOGViewModel)parameter ?? throw new ArgumentNullException("logViewModel", "DeleteCacheItemCommand<TLOGViewModel>");
                await HandleCodingCacheItem(logViewModel);
            }
            catch (InvalidCastException castx)
            {
                await _dataService.HandleExceptionAsync(castx);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }


        private async Task HandleCodingCacheItem(TLOGViewModel item)
        {
            var tempCodingDashboardItems = _logCacheViewModel.CacheItems;

            if (_timeUp)
            {
                // Send data to database first
                var codingCacheDTO = (CodingCacheDTO)_logCacheDTO;
                bool isLogged = await _dataService.CreateLOGAsync(codingCacheDTO);

                if (isLogged)
                {
                    tempCodingDashboardItems.Remove(item);
                    _logCacheViewModel.CacheItems = tempCodingDashboardItems;
                    await _logCacheViewModel.UpdateLogCount();

                }
            }
            else
            {
                var context = _logCacheDTO.Context;

                _cacheService.DeleteCacheFile(_logCacheDTO.UserId, _logCacheDTO.Start.ToString("yyyyMMdd_HHmmss"), context);

                tempCodingDashboardItems.Remove(item);
                _logCacheViewModel.CacheItems = tempCodingDashboardItems;
                await _logCacheViewModel.UpdateLogCount();
            }
        }



    }
}
