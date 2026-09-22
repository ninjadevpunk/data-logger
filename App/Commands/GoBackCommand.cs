using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class GoBackCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GoBackCommand(NavigationService navigationService, MainWindowViewModel mainWindowViewModel)
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
                _navigationService.GoBack(false);
            }
            catch (Exception)
            {
                //
            }



        }
    }
}
