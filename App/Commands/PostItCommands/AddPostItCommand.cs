using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class AddPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerViewModel _loggerCreateViewModel;

        public AddPostItCommand(NavigationService navigationService, LoggerViewModel loggerCreateViewModel)
        {


            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            }
            catch (Exception)
            {
                //
            }


        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.NavigateToPostItCreator(_loggerCreateViewModel);
        }
    }
}
