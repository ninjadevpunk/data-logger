using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class ForgotPasswordCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public ForgotPasswordCommand()
        {
            
        }

        public ForgotPasswordCommand(NavigationService navigationService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            }
            catch (Exception ex)
            {
                //
            }
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
