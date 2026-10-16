using Core.DTOs.Cache;
using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class DeleteCacheItemCommand : AsyncCommandBase
    {
        private readonly LogCacheDTO _logCacheDTO;
        private readonly LogCacheViewModel _logCacheViewModel;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly bool _timeUp;

        public DeleteCacheItemCommand(LogCacheViewModel logCacheViewModel, IDataService dataService, ICacheService cacheService,
            LogCacheDTO logCacheDTO, bool timeIsUp)
        {
            _logCacheViewModel = logCacheViewModel;
            _dataService = dataService;
            _cacheService = cacheService;
            _logCacheDTO = logCacheDTO;
            _timeUp = timeIsUp;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var logViewModel = (LOGViewModel)parameter ?? throw new ArgumentNullException(null, "DeleteCacheItemCommand");
                await SendCodingCacheToDatabase(logViewModel);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }


        private async Task SendCodingCacheToDatabase(LOGViewModel item)
        {
            var cachedDashboardItems = _logCacheViewModel.CacheItems;

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
                                _cacheService.DeleteCacheFile(_logCacheDTO.UserId, _logCacheDTO.Start.ToString("yyyyMMdd_HHmmss"),
                                    _logCacheDTO.Context);
                                await UpdateDashboard(cachedDashboardItems, item);
                            }
                        }
                        else
                        {
                            item.Dispose();
                            await UpdateDashboard(cachedDashboardItems, item);
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
                                _cacheService.DeleteCacheFile(_logCacheDTO.UserId, _logCacheDTO.Start.ToString("yyyyMMdd_HHmmss"),
                                    _logCacheDTO.Context);
                                await UpdateDashboard(cachedDashboardItems, item);
                            }
                        }
                        else
                        {
                            item.Dispose();
                            await UpdateDashboard(cachedDashboardItems, item);
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
                                _cacheService.DeleteCacheFile(_logCacheDTO.UserId, _logCacheDTO.Start.ToString("yyyyMMdd_HHmmss"), 
                                    _logCacheDTO.Context);
                                await UpdateDashboard(cachedDashboardItems, item);
                            }
                        }
                        else
                        {
                            item.Dispose();
                            await UpdateDashboard(cachedDashboardItems, item);
                        }

                        break;
                    }
            }

        }



        private async Task UpdateDashboard(ObservableCollection<LOGViewModel> logViewModels, LOGViewModel itemLogViewModel)
        {
            logViewModels.Remove(itemLogViewModel);
            _logCacheViewModel.CacheItems = logViewModels;
            await _logCacheViewModel.UpdateLogCount();
        }


    }
}
