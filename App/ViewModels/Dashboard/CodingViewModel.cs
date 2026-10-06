using Core.Enums;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    /// <summary>
    /// Represents UI managing coding log cache.
    /// </summary>
    internal class CodingViewModel : LogCacheViewModel
    {
        public override LogContext Context => LogContext.Coding;

        public CodingViewModel(NavigationService navigationService, IDataService _dataService) 
            : base(navigationService, _dataService)
        {
            CacheItems = [];
        }

        

        internal override async Task UpdateLogCount()
        {
            var count = await _dataService.LogCountAsync(LOG.CATEGORY.CODING);

            if (count == -1)
                throw new InvalidOperationException($"Count failed in {LogContext.Coding} UpdateLogCount");

            LogCount = $"{CacheItems.Count} coding logs cached | {count} total logs";
        }


        /// <summary>
        /// Retrieves coding log cache as read-only list.
        /// </summary>
        public IReadOnlyList<CodeLOGViewModel> CodingCacheItems => [.. CacheItems.Cast<CodeLOGViewModel>()];
    }
}
