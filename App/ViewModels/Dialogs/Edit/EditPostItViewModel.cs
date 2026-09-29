using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Commands.PostItCommands.PostCommand;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt
{
    public class EditPostItViewModel : PostItViewModel
    {
        public EditPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, LOG.CATEGORY category,
            PostItViewModel createPostItViewModel) :
            base(navigationService, dataService, loggerCreateViewModel, category)
        {
            Subject = createPostItViewModel.Subject;
            PlaceholderText = string.IsNullOrEmpty(Subject) ? "Subject" : string.Empty;

            Error = createPostItViewModel.Error;
            Solution = createPostItViewModel.Solution;
            Suggestion = createPostItViewModel.Suggestion;
            Comment = createPostItViewModel.Comment;

            Display_Error = createPostItViewModel.Display_Error;
            Display_Solution = createPostItViewModel.Display_Solution;
            Display_Suggestion = createPostItViewModel.Display_Suggestion;
            Display_Comment = createPostItViewModel.Display_Comment;

            PostCommand = new PostCommand(ActionType.Edit, PostItContext.EDITPOSTIT, navigationService, loggerCreateViewModel, this, createPostItViewModel);
        }

        public EditPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, ProjectClass project,
            PostItViewModel createPostItViewModel) :
            base(navigationService, dataService, loggerCreateViewModel, project)
        {
            Subject = createPostItViewModel.Subject;
            Subjects = createPostItViewModel.Subjects;

            Error = createPostItViewModel.Error;
            Solution = createPostItViewModel.Solution;
            Suggestion = createPostItViewModel.Suggestion;
            Comment = createPostItViewModel.Comment;

            Display_Error = createPostItViewModel.Display_Error;
            Display_Solution = createPostItViewModel.Display_Solution;
            Display_Suggestion = createPostItViewModel.Display_Suggestion;
            Display_Comment = createPostItViewModel.Display_Comment;

            PostCommand = new PostCommand(ActionType.Edit, PostItContext.EDITPOSTIT, navigationService, loggerCreateViewModel, this, createPostItViewModel);
        }
    }
}
