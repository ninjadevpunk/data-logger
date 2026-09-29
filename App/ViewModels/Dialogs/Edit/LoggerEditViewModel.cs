using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal abstract class LoggerEditViewModel : LoggerViewModel
    {
        public override LogContext Context { get; }
        public override LOG.CATEGORY Category { get; }
        protected override bool AppFieldEnabled => true;

        protected readonly ICacheService _cacheService;

        public LoggerEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IAuthService authService,
            IDataService dataService, ICacheService cacheService)
            : base(navigationService, logCacheViewModel, authService, dataService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService), "LoggerCreateViewModel");
        }
    }
}
