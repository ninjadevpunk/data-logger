using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Dialogs.Updater
{
    internal abstract class CodingLoggerUpdateViewModel : CodingReporterLoggerViewModel
    {
        protected CodingLoggerUpdateViewModel(NavigationService navigationService, IAuthService authService,
            IDataService dataService)
            : base(navigationService, authService, dataService)
        {
        }

    }
}
