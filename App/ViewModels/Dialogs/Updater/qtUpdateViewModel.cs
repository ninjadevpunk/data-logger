using Core.DTOs.Data;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Updater
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class qtUpdateViewModel : CodingLoggerUpdateViewModel
    {
        public override ICommand UpdateLogCommand => throw new NotImplementedException();
        public override ICommand SaveCommand => throw new NotImplementedException();


        public qtUpdateViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService, QtDataDTO qtDataDTO) 
            : base(navigationService, authService, dataService)
        {
        }

        public override Task UpdateLogCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
