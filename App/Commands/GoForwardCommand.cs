using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    internal class GoForwardCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GoForwardCommand(NavigationService navigationService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                // TODO
                //_navigationService.GoForward();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} GoForwardCommand.Execute: {ex.Message}");
            }
        }
    }
}
