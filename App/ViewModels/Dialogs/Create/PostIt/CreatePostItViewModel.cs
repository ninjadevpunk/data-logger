using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt
{
    internal sealed class CreatePostItViewModel : PostItViewModel
    {
        // TODO: Implement PostIt replacer command
        public override ICommand OKCommand => throw new NotImplementedException();
        public override ICommand PostItCommand => new PostItCommand(_navigationService, _loggerCreateViewModel, this);

        public CreatePostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel) 
            : base(navigationService, dataService, loggerCreateViewModel)
        {
        }

    }
}
