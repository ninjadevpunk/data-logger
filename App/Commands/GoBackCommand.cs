using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    internal class GoBackCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public GoBackCommand(NavigationService navigationService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {

            try
            {
                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} GoBackCommand.Execute: {ex.Message}");
            }
        }
    }
}
