using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Commands.PostItCommands.ToolCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class PostItViewModel : ViewModelBase
    {
        public enum PostItField { Error, Solution, Suggestion, Comment }

        protected readonly NavigationService? _navigationService;
        protected readonly IDataService? _dataService;
        protected readonly LoggerCreateViewModel? _loggerCreateViewModel;
        

        public PostItViewModel()
        {
            
        }

        public PostItViewModel(NavigationService navigationService, IDataService dataService, LoggerCreateViewModel loggerCreateViewModel, LOG.CATEGORY category)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;

            Subject = "";

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


            EditCommand = new EditPostItCommand(_navigationService, loggerCreateViewModel);
            PostCommand = new PostCommand(_navigationService, _loggerCreateViewModel, this);
            DeletePostItCommand = new DeletePostItCommand(_loggerCreateViewModel);
            EraserCommand = new EraserCommand(this);
            HighlighterCommand = new HighlighterCommand(this);
        }

        public PostItViewModel(NavigationService navigationService, IDataService dataService, LoggerCreateViewModel loggerCreateViewModel, ProjectClass project)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;

            Subject = "";

            Subjects = new();
            foreach (SubjectClass subject in _dataService.SQLITE_SUBJECTS)
            {
                Subjects.Add(subject.Subject);
            }

            ErrorVisible = Visibility.Collapsed;
            SolutionVisible = Visibility.Collapsed;
            SuggestionVisible = Visibility.Collapsed;
            CommentVisible = Visibility.Collapsed;

            Error = "";
            Solution = "";
            Suggestion = "";
            Comment = "";

            Option1Check = true;

            EditCommand = new EditPostItCommand(_navigationService, loggerCreateViewModel);
            PostCommand = new PostCommand(_navigationService, _loggerCreateViewModel, this);
            DeletePostItCommand = new DeletePostItCommand(_loggerCreateViewModel);
            EraserCommand = new EraserCommand(this);
            HighlighterCommand = new HighlighterCommand(this);
        }

        public PostItViewModel(NavigationService navigationService, IDataService dataService, LoggerCreateViewModel loggerCreateViewModel, ProjectClass project, 
            string subject, string error, DateTime dateFound, string solution, DateTime dateSolved, string suggestion, string comment)
        {
            _navigationService = navigationService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _dataService = dataService;


            Subject = subject;

            Error = error;
            Display_Error = ConvertRtfToPlainText(error);
            DateFound = dateFound;

            Solution = solution;
            Display_Solution = ConvertRtfToPlainText(solution);
            DateSolved = dateSolved;

            Suggestion = suggestion;
            Display_Suggestion = ConvertRtfToPlainText(suggestion);
            Comment = comment;
            Display_Comment = ConvertRtfToPlainText(comment);

            ErrorVisible = Error.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;
            SolutionVisible = Solution.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;
            SuggestionVisible = Suggestion.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;
            CommentVisible = Comment.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;

            Option1Check = true;

            EditCommand = new EditPostItCommand(_navigationService, loggerCreateViewModel);
            PostCommand = new PostCommand(_navigationService, _loggerCreateViewModel, this);
            DeletePostItCommand = new DeletePostItCommand(_loggerCreateViewModel);
            EraserCommand = new EraserCommand(this);
            HighlighterCommand = new HighlighterCommand(this);
        }

        

        public PostItViewModel(NavigationService navigationService, ProjectClass project, string subject, string error,
            string solution, string suggestion, string comment)
        {
            _navigationService = navigationService;

            Subject = subject;

            Error = error;
            Display_Error = ConvertRtfToPlainText(error);
            ErrorTextBlockHeight = string.IsNullOrEmpty(Display_Error) || Display_Error.Length < 64 ? 24 : double.NaN;

            Solution = solution;
            Display_Solution = ConvertRtfToPlainText(solution);
            SolutionTextBlockHeight = string.IsNullOrEmpty(Display_Solution) || Display_Solution.Length < 64 ? 24 : double.NaN;

            Suggestion = suggestion;
            Display_Suggestion = ConvertRtfToPlainText(suggestion);
            SuggestionTextBlockHeight = string.IsNullOrEmpty(Display_Suggestion) || Display_Suggestion.Length < 64 ? 24 : double.NaN;

            Comment = comment;
            Display_Comment = ConvertRtfToPlainText(comment);
            CommentTextBlockHeight = string.IsNullOrEmpty(Display_Comment) || Display_Comment.Length < 64 ? 24 : double.NaN;

            ErrorVisible = Error.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;
            SolutionVisible = Solution.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;
            SuggestionVisible = Suggestion.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;
            CommentVisible = Comment.Equals(string.Empty) ? Visibility.Collapsed : Visibility.Visible;

            Option1Check = true;
        }









        public virtual async Task AutoStartAsync(ProjectClass project)
        {
            await _dataService.InitialiseSubjectsLIST(project ?? new("Unnamed Project"));
        }
        public bool FoundDateCaptured { get; set; } = false;
        public bool SolvedDateCaptured { get; set; } = false;


        public int ErrorRtfLength { get; set; } = 0;
        public int SolutionRtfLength { get; set; } = 0;
        public int SuggestionRtfLength { get; set; } = 0;
        public int CommentRtfLength { get; set; } = 0;
        public int BackspaceConstant { get; } = 274;

        public bool ErrorLengthIsSet { get; set; } = false;
        public bool SolutionLengthIsSet { get; set; } = false;
        public bool SuggestionLengthIsSet { get; set; } = false;
        public bool CommentLengthIsSet { get; set; } = false;


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

        private RichTextBox activeEditor;
        public RichTextBox ActiveEditor
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

        private string subject;
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


        private HashSet<string> subjects;
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

        private string error;
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;

                if (!FoundDateCaptured)
                    DateFound = DateTime.Now;

                ErrorVisible = Error != "" ? Visibility.Visible : Visibility.Collapsed;


                if (!ErrorLengthIsSet)
                {
                    ErrorRtfLength = Error.Length - 1;
                    ErrorLengthIsSet = true;

                    if (ErrorRtfLength < 0)
                    {
                        ErrorRtfLength = 0;
                        ErrorLengthIsSet = false;
                    }
                }

                if (ErrorLengthIsSet && Error.Length == ErrorRtfLength || ErrorLengthIsSet && Error.Length == BackspaceConstant)
                {
                    Error = "";
                }

                OnPropertyChanged(nameof(Error));
            }
        }



        private DateTime dateFound;
        public DateTime DateFound
        {
            get
            {
                return dateFound;
            }
            set
            {

                if (!FoundDateCaptured)
                {
                    dateFound = value;
                    FoundDateCaptured = true;
                    OnPropertyChanged(nameof(DateFound));
                }

            }
        }

        public double ErrorTextBlockHeight { get; set; }
        public double SolutionTextBlockHeight { get; set; }
        public double SuggestionTextBlockHeight { get; set; }
        public double CommentTextBlockHeight { get; set; }


        private string display_error;
        public string Display_Error
        {
            get
            {
                return display_error;
            }
            set
            {
                display_error = value;
                OnPropertyChanged(nameof(Display_Error));
            }
        }

        private string solution;
        public string Solution
        {
            get
            {
                return solution;
            }
            set
            {
                solution = value;

                if (!SolvedDateCaptured)
                    DateSolved = DateTime.Now;


                SolutionVisible = Solution != "" ? Visibility.Visible : Visibility.Collapsed;

                if (!SolutionLengthIsSet)
                {
                    SolutionRtfLength = Solution.Length - 1;
                    SolutionLengthIsSet = true;

                    if (SolutionRtfLength < 0)
                    {
                        SolutionRtfLength = 0;
                        SolutionLengthIsSet = false;
                    }
                }

                if (SolutionLengthIsSet && Solution.Length == SolutionRtfLength || SolutionLengthIsSet && Solution.Length == BackspaceConstant)
                {
                    Solution = "";
                }


                OnPropertyChanged(nameof(Solution));
            }
        }

        private DateTime dateSolved;
        public DateTime DateSolved
        {
            get
            {
                return dateSolved;
            }
            set
            {
                if (!SolvedDateCaptured)
                {
                    dateSolved = value;
                    SolvedDateCaptured = true;
                    OnPropertyChanged(nameof(DateSolved));
                }
            }
        }

        private string display_solution;
        public string Display_Solution
        {
            get
            {
                return display_solution;
            }
            set
            {
                display_solution = value;
                OnPropertyChanged(nameof(Display_Solution));
            }
        }

        private string suggestion;
        public string Suggestion
        {
            get
            {
                return suggestion;
            }
            set
            {
                suggestion = value;
                SuggestionVisible = Suggestion != "" ? Visibility.Visible : Visibility.Collapsed;

                if (!SuggestionLengthIsSet)
                {
                    SuggestionRtfLength = Suggestion.Length - 1;
                    SuggestionLengthIsSet = true;

                    if (SuggestionRtfLength < 0)
                    {
                        SuggestionRtfLength = 0;
                        SuggestionLengthIsSet = false;
                    }
                }

                if (SuggestionLengthIsSet && Suggestion.Length == SuggestionRtfLength || SuggestionLengthIsSet && Suggestion.Length == BackspaceConstant)
                {
                    Suggestion = "";
                }

                OnPropertyChanged(nameof(Suggestion));
            }
        }

        private string display_suggestion;
        public string Display_Suggestion
        {
            get
            {
                return display_suggestion;
            }
            set
            {
                display_suggestion = value;
                OnPropertyChanged(nameof(Display_Suggestion));
            }
        }

        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                CommentVisible = Comment != "" ? Visibility.Visible : Visibility.Collapsed;


                if (!CommentLengthIsSet)
                {
                    CommentRtfLength = Comment.Length - 1;
                    CommentLengthIsSet = true;

                    if (CommentRtfLength < 0)
                    {
                        CommentRtfLength = 0;
                        CommentLengthIsSet = false;
                    }
                }

                if (CommentLengthIsSet && Comment.Length == CommentRtfLength || CommentLengthIsSet && Comment.Length == BackspaceConstant)
                {
                    Comment = "";
                }


                OnPropertyChanged(nameof(Comment));
            }
        }

        private string display_comment;
        public string Display_Comment
        {
            get
            {
                return display_comment;
            }
            set
            {
                display_comment = value;
                OnPropertyChanged(nameof(Display_Comment));
            }
        }

        private Visibility errorVisible;
        public Visibility ErrorVisible
        {
            get
            {
                return errorVisible;
            }
            set
            {
                errorVisible = value;
                OnPropertyChanged(nameof(ErrorVisible));
            }
        }

        private Visibility solutionVisible;
        public Visibility SolutionVisible
        {
            get
            {
                return solutionVisible;
            }
            set
            {
                solutionVisible = value;
                OnPropertyChanged(nameof(SolutionVisible));
            }
        }

        private Visibility suggestionVisible;
        public Visibility SuggestionVisible
        {
            get
            {
                return suggestionVisible;
            }
            set
            {
                suggestionVisible = value;
                OnPropertyChanged(nameof(SuggestionVisible));
            }
        }

        private Visibility commentVisible;
        public Visibility CommentVisible
        {
            get
            {
                return commentVisible;
            }
            set
            {
                commentVisible = value;
                OnPropertyChanged(nameof(CommentVisible));
            }
        }


        // REPLACER 

        private string replace;
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


        public ICommand OKCommand { get; set; }

        public ICommand PostCommand { get; set; }

        public ICommand DeletePostItCommand { get; set; }

        public ICommand EditCommand { get; set; }


        #region Toolbar




        public ICommand EraserCommand { get; set; }

        public ICommand HighlighterCommand { get; set; }

        public ICommand FontCommand { get; set; }

        public ICommand ULListCommand { get; set; }

        public ICommand OLListCommand { get; set; }

        public ICommand BoldCommand { get; set; }

        public ICommand ItalicsCommand { get; set; }

        public ICommand UnderlineCommand { get; set; }




        #endregion




        #region Member Functions



        public static string ConvertRtfToPlainText(string rtfContent)
        {
            try
            {
                // Create a temporary RichTextBox
                var richTextBox = new RichTextBox();

                // Convert RTF content to a byte array
                byte[] rtfBytes = Encoding.UTF8.GetBytes(rtfContent);

                // Use a MemoryStream to load the RTF content
                using (var stream = new MemoryStream(rtfBytes))
                {
                    richTextBox.Document = new FlowDocument();
                    var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    textRange.Load(stream, DataFormats.Rtf);
                }

                // Extract plain text
                string plainText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                return plainText;
            }
            catch (ArgumentException argx)
            {
                Debug.WriteLine($"ArgumentException near ConvertRtfToPlaintText(): {argx.Message}");
                
                return string.Empty; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near ConvertRtfToPlainText(): {ex.Message}");

                // TODO

                return string.Empty;
            }
        }

        public async Task LoadSubjectsAsync(LOG.CATEGORY category)
        {
            try
            {
                await _dataService.InitialiseSubjectsLIST(category);

                Subjects = new();
                Subjects.Clear();
                foreach (SubjectClass subject in _dataService.SQLITE_SUBJECTS)
                {
                    Subjects.Add(subject.Subject);
                }
            }
            catch (Exception e)
            {
                //
            }
        }







        #endregion

    }
}
