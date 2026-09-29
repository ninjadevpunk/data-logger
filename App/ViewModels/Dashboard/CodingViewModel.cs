using Core.Enums;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    internal class CodingViewModel : LogCacheViewModel
    {
        public override ICommand CreateLogCommand => new CreateLogCommand(_navigationService, _dataService);
        public override ICommand ReportLogCommand => new ReportCommand(_navigationService);
        public override LogContext Context => LogContext.Coding;

        public CodingViewModel(NavigationService navigationService, IDataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = [];
        }

        public async Task AutoStartAsync()
        {
            await UpdateLogCount();
        }

        public void OnLogOut()
        {
            CacheItems.Clear();
            LogCount = "";
        }

        public override async Task UpdateLogCount()
        {
            var count = await _dataService.LogCountAsync(LOG.CATEGORY.CODING);

            if (count == -1)
                throw new InvalidOperationException($"Count failed in {LogContext.Coding} UpdateLogCount");

            LogCount = $"{CacheItems.Count} coding logs cached | {count} total logs";
        }

        public IReadOnlyList<CodeLOGViewModel> CodingCacheItems => CacheItems.Cast<CodeLOGViewModel>().ToList();
    }
}
