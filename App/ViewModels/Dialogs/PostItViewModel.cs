using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands.ToolCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    internal abstract class PostItViewModel : ViewModelBase
    {
        public enum PostItField { Error, Solution, Suggestion, Comment }

        protected readonly NavigationService _navigationService = null!;
        protected readonly IDataService _dataService = null!;
        protected readonly LoggerViewModel _loggerCreateViewModel = null!;



        public PostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;

            Subject = "";
            Subjects = [];


            Error = "";
            Solution = "";
            Suggestion = "";
            Comment = "";

            Option1Check = true;


            EraserCommand = new EraserCommand(this);
            HighlighterCommand = new HighlighterCommand(this);
        }





        public virtual async Task AutoStartAsync(string appName, string projectName)
        {
            Subjects = [.. (await _dataService.InitialiseSubjectsLISTAsync(appName, projectName))];
        }


        public string PlaceholderText { get; set; } = "Subject";




        #region Toolbar Properties



        private int userSelection;
        public int UserSelection
        {
            get
            {
                return userSelection;
            }
            set
            {
                userSelection = value;

                switch (UserSelection)
                {
                    case 0:
                        FieldToEdit = PostItField.Error;
                        break;
                    case 1:
                        FieldToEdit = PostItField.Solution;
                        break;
                    case 2:
                        FieldToEdit = PostItField.Suggestion;
                        break;
                    case 3:
                        FieldToEdit = PostItField.Comment;
                        break;
                }

                OnPropertyChanged(nameof(UserSelection));
            }
        }

        private PostItField fieldToEdit;
        public PostItField FieldToEdit
        {
            get
            {
                return fieldToEdit;
            }
            set
            {
                fieldToEdit = value;
                OnPropertyChanged(nameof(FieldToEdit));
            }
        }

        private RichTextBox? activeEditor;
        public RichTextBox? ActiveEditor
        {
            get
            {
                return activeEditor;
            }
            set
            {
                activeEditor = value;
                OnPropertyChanged(nameof(ActiveEditor));
            }
        }





        #endregion







        private string subject = string.Empty;
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }


        private HashSet<string> subjects = [];
        public HashSet<string> Subjects
        {
            get
            {
                return subjects;
            }
            set
            {
                subjects = value;
                OnPropertyChanged(nameof(Subjects));
            }
        }

        private string error = string.Empty;
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }


        private string solution = string.Empty;
        public string Solution
        {
            get
            {
                return solution;
            }
            set
            {
                solution = value;
                OnPropertyChanged(nameof(Solution));
            }
        }


        private string suggestion = string.Empty;
        public string Suggestion
        {
            get
            {
                return suggestion;
            }
            set
            {
                suggestion = value;
                OnPropertyChanged(nameof(Suggestion));
            }
        }


        private string comment = string.Empty;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }



        // REPLACER 

        private string replace = string.Empty;
        public string Replace
        {
            get
            {
                return replace;
            }
            set
            {
                replace = value;
                OnPropertyChanged(nameof(Replace));
            }
        }

        private bool option1Check;
        public bool Option1Check
        {
            get
            {
                return option1Check;
            }
            set
            {
                option1Check = value;
                OnPropertyChanged(nameof(Option1Check));
            }
        }

        private bool option2Check;
        public bool Option2Check
        {
            get
            {
                return option2Check;
            }
            set
            {
                option2Check = value;
                OnPropertyChanged(nameof(Option2Check));
            }
        }

        private bool option3Check;
        public bool Option3Check
        {
            get
            {
                return option3Check;
            }
            set
            {
                option3Check = value;
                OnPropertyChanged(nameof(Option3Check));
            }
        }

        private bool option4Check;
        public bool Option4Check
        {
            get
            {
                return option4Check;
            }
            set
            {
                option4Check = value;
                OnPropertyChanged(nameof(Option4Check));
            }
        }


        /// <summary>
        /// Command to execute when the OK button is clicked. This command should handle replacer logic.
        /// </summary>
        public abstract ICommand OKCommand { get; }

        /// <summary>
        /// Command to execute when the Post button is clicked. This command should handle posting the data to the database.
        /// </summary>
        public abstract ICommand PostItCommand { get; }


        #region Toolbar




        public ICommand EraserCommand { get; set; } = null!;

        public ICommand HighlighterCommand { get; set; } = null!;

        public ICommand FontCommand { get; set; } = null!;

        public ICommand ULListCommand { get; set; } = null!;

        public ICommand OLListCommand { get; set; } = null!;

        public ICommand BoldCommand { get; set; } = null!;

        public ICommand ItalicsCommand { get; set; } = null!;

        public ICommand UnderlineCommand { get; set; } = null!;




        #endregion





    }
}
