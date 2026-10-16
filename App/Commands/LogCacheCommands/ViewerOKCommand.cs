using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class ViewerOKCommand : CommandBase
    {
        private readonly NavigationService _navigationService;


        public ViewerOKCommand(NavigationService navigationService)
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
                Debug.WriteLine($"Exception occurred in ViewerOKCommand.Execute: {ex.Message}");
            }
        }
    }
}
