using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class EditPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;


        public EditPostItCommand()
        {

        }

        public EditPostItCommand(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            }
            catch (ArgumentNullException ex)
            {
                // TODO
            }
            catch (Exception)
            {
                //
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToPostItCreator(_loggerCreateViewModel, parameter as PostItViewModel);
            }
            catch (ArgumentNullException nullex)
            {
                //
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
