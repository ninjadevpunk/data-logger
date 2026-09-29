using System.Windows;
using Core.Models.App_Models;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Services;
namespace Data_Logger_1._3.ViewModels.Reporter.Updater
{
    public class EF_EditPostItViewModel : EF_PostItViewModel
    {
        /// <summary>
        /// This method is for creating PostIts in the reporter updater.
        /// </summary>
        /// <param name="navigationService">The navigation service to take the user back to the updater</param>
        /// <param name="dataService">The data service to populate the subjects list</param>
        /// <param name="reporterUpdaterViewModel">The updater instance resolved from DI</param>
        public EF_EditPostItViewModel(NavigationService navigationService, IDataService dataService, ReporterUpdaterViewModel reporterUpdaterViewModel) :
            base(navigationService, dataService, reporterUpdaterViewModel)
        {
            Subject = string.Empty;
            PlaceholderText = string.IsNullOrEmpty(Subject) ? "Subject" : string.Empty;

            Subjects = new();

            ErrorVisible = Visibility.Collapsed;
            SolutionVisible = Visibility.Collapsed;
            SuggestionVisible = Visibility.Collapsed;
            CommentVisible = Visibility.Collapsed;

            Error = "";
            Solution = "";
            Suggestion = "";
            Comment = "";

            Option1Check = true;

            PostCommand = new EF_PostCommand(navigationService, reporterUpdaterViewModel, this);
        }



        /// <summary>
        /// This method is for editing PostIts in the reporter updater.
        /// </summary>
        /// <param name="id">The PostIt ID given by EntityFramework</param>
        /// <param name="navigationService">The navigation service to take the user back to the updater</param>
        /// <param name="dataService">The data service to populate the subjects list</param>
        /// <param name="ReporterUpdaterViewModel">The updater instance resolved from DI</param>
        /// <param name="project">The project instance needed for the subjects list retrieval</param>
        /// <param name="postItViewModel">A PostIt viewmodel to populate fields</param>
        public EF_EditPostItViewModel(int id, NavigationService navigationService, IDataService dataService, 
            ReporterUpdaterViewModel reporterUpdaterViewModel, ProjectClass project, EF_PostItViewModel postItViewModel) :
            base(navigationService, dataService, reporterUpdaterViewModel, project)
        {
            ID = id;
            Subject = postItViewModel.Subject;
            PlaceholderText = string.IsNullOrEmpty(Subject) ? "Subject" : string.Empty;

            Subjects = postItViewModel.Subjects;

            Error = postItViewModel.Error;
            Solution = postItViewModel.Solution;
            Suggestion = postItViewModel.Suggestion;
            Comment = postItViewModel.Comment;

            Display_Error = postItViewModel.Display_Error;
            Display_Solution = postItViewModel.Display_Solution;
            Display_Suggestion = postItViewModel.Display_Suggestion;
            Display_Comment = postItViewModel.Display_Comment;

            EditCommand = new EF_EditPostItCommand(navigationService, reporterUpdaterViewModel);
            PostCommand = new EF_PostCommand(EF_PostCommand.ActionType.Edit, navigationService, reporterUpdaterViewModel, this, postItViewModel);
        }
    }
}
