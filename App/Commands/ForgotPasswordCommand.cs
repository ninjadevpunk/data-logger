using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    internal class ForgotPasswordCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public ForgotPasswordCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            // TODO: Implement forgot password.
            throw new NotImplementedException();
        }
    }
}
