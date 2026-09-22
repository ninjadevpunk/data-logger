using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class GoForwardCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GoForwardCommand(NavigationService navigationService, MainWindowViewModel mainWindowViewModel)
        {



            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
            }
            catch (Exception)
            {
                //
            }



        }

        public override void Execute(object parameter)
        {


            try
            {
                //_navigationService.GoForward();
            }
            catch (Exception)
            {
                //
            }



        }
    }
}
