using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.Utilities
{
    internal static class Extensions
    {
        public static async Task<string> GetLogCountAsync(IDataService dataService, CodingViewModel codingViewModel, LOG.CATEGORY category)
        {
            var count = await dataService.LogCountAsync(category);
            codingViewModel.LogCount = $"{codingViewModel.CacheItems.Count} coding logs cached | {count} total logs";

            return $"{codingViewModel.CacheItems.Count} Logs Cached";
        }
    }
}
