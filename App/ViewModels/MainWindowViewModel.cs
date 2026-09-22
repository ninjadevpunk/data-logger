using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Commands.SettingsCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Data_Logger_1._3.Services.CacheMaster;
using static Data_Logger_1._3.Services.NavigationService;

namespace Data_Logger_1._3.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;


        public MainWindowViewModel(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;

            ShowDefault = Visibility.Visible;

            MenuActivated = true;

            CodingChecked = true;
            CodingQtChecked = true;
            NotesChecked = false;

            GoBackCommand = new GoBackCommand(_navigationService, this);
            GoForwardCommand = new GoForwardCommand(_navigationService, this);
            LogOutCommand = new LogOutCommand(_navigationService);
            SettingsCommand = new NavigateToSettingsCommand(_navigationService, dataService);
            OpenUpdaterCommand = new OpenUpdaterCommand(_navigationService, dataService);

            IconBackFill = DisabledColor;
            IconForwardFill = DisabledColor;
        }





        private static readonly Brush? EnabledColor = UIFactory.TryParseBrush("iconCOLOURAccent01");
        private static readonly Brush? DisabledColor = UIFactory.TryParseBrush("AccentColour");


        private Brush? iconBackFill;
        public Brush? IconBackFill
        {
            get
            {
                return iconBackFill;
            }
            set
            {
                iconBackFill = value;
                OnPropertyChanged(nameof(IconBackFill));
            }
        }

        private Brush? iconForwardFill;
        public Brush? IconForwardFill
        {
            get
            {
                return iconForwardFill;
            }
            set
            {
                iconForwardFill = value;
                OnPropertyChanged(nameof(IconForwardFill));
            }
        }




        private bool backEnabled;
        public bool BackEnabled
        {
            get
            {
                return backEnabled;
            }
            set
            {
                backEnabled = value;
                IconBackFill = BackEnabled ? EnabledColor : DisabledColor;
                OnPropertyChanged(nameof(BackEnabled));
            }
        }

        private bool forwardEnabled;
        public bool ForwardEnabled
        {
            get
            {
                return forwardEnabled;
            }
            set
            {
                forwardEnabled = value;
                IconForwardFill = ForwardEnabled ? EnabledColor : DisabledColor;
                OnPropertyChanged(nameof(ForwardEnabled));
            }
        }

        private bool menuActivated;
        public bool MenuActivated
        {
            get
            {
                return menuActivated;
            }
            set
            {
                menuActivated = value;
                OnPropertyChanged(nameof(MenuActivated));
            }
        }


        private bool codingChecked;
        public bool CodingChecked
        {
            get
            {
                return codingChecked;
            }
            set
            {
                if (codingChecked != value)
                {
                    codingChecked = value;
                    OnPropertyChanged(nameof(CodingChecked));

                    if (codingChecked)
                    {
                        // Navigate to the Coding page
                        _navigationService.NavigateToLogCachePage();

                        if (!CodingQtChecked)
                            CodingQtChecked = true;

                        UncheckButtons();
                    }
                }
            }
        }


        private bool codingQtChecked;
        public bool CodingQtChecked
        {
            get
            {
                return codingQtChecked;
            }
            set
            {
                if (codingQtChecked != value)
                {
                    codingQtChecked = value;
                    OnPropertyChanged(nameof(CodingQtChecked));
                }

            }
        }


        private bool codingAndroidChecked;
        public bool CodingAndroidChecked
        {
            get
            {
                return codingAndroidChecked;
            }
            set
            {
                if (codingAndroidChecked != value)
                {
                    codingAndroidChecked = value;
                    OnPropertyChanged(nameof(CodingAndroidChecked));
                }
            }
        }


        private bool codingGenericChecked;
        public bool CodingGenericChecked
        {
            get
            {
                return codingGenericChecked;
            }
            set
            {
                if (codingGenericChecked != value)
                {
                    codingGenericChecked = value;
                    OnPropertyChanged(nameof(CodingGenericChecked));
                }
            }
        }


        private bool graphicsChecked;
        public bool GraphicsChecked
        {
            get
            {
                return graphicsChecked;
            }
            set
            {
                if (graphicsChecked != value)
                {
                    graphicsChecked = value;
                    OnPropertyChanged(nameof(GraphicsChecked));

                    if (graphicsChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Graphics);
                        UncheckButtons();

                    }
                }
            }
        }


        private bool filmChecked;
        public bool FilmChecked
        {
            get
            {
                return filmChecked;
            }
            set
            {
                if (filmChecked != value)
                {
                    filmChecked = value;
                    OnPropertyChanged(nameof(FilmChecked));

                    if (filmChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Film);
                        UncheckButtons();

                    }
                }
            }
        }


        private bool notesChecked;
        public bool NotesChecked
        {
            get
            {
                return notesChecked;
            }
            set
            {
                if (notesChecked != value)
                {
                    notesChecked = value;
                    OnPropertyChanged(nameof(NotesChecked));

                }
            }
        }


        private bool genericNotesChecked;
        public bool GenericNotesChecked
        {
            get
            {
                return genericNotesChecked;
            }
            set
            {
                if (genericNotesChecked != value)
                {
                    genericNotesChecked = value;
                    OnPropertyChanged(nameof(GenericNotesChecked));
                }
            }
        }


        private bool checklistNotesChecked;
        public bool ChecklistNotesChecked
        {
            get
            {
                return checklistNotesChecked;
            }
            set
            {
                if (checklistNotesChecked != value)
                {
                    checklistNotesChecked = value;
                    OnPropertyChanged(nameof(ChecklistNotesChecked));

                    if (checklistNotesChecked)
                    {
                        //_ = _navigationService.NavigateToCreateCheckListPage();
                    }
                }
            }
        }


        private bool flexiChecked;
        public bool FlexiChecked
        {
            get
            {
                return flexiChecked;
            }
            set
            {
                if (flexiChecked != value)
                {
                    flexiChecked = value;
                    OnPropertyChanged(nameof(FlexiChecked));

                    if (flexiChecked)
                    {
                        //_ = _navigationService.NavigateToLogCachePage(CacheContext.Flexi);
                    }
                }
            }
        }


        private BitmapImage? signupImage = null!;
        public BitmapImage? SignUpImage
        {
            get
            {
                return signupImage;
            }
            set
            {
                signupImage = value;

                if (SignUpImage != null)
                    ShowDefault = Visibility.Collapsed;
                else
                    ShowDefault = Visibility.Visible;

                OnPropertyChanged(nameof(SignUpImage));
            }
        }


        private Visibility showDefault;
        public Visibility ShowDefault
        {
            get
            {
                return showDefault;
            }
            set
            {
                showDefault = value;
                OnPropertyChanged(nameof(ShowDefault));
            }
        }

        private Visibility updaterButtonVisible = Visibility.Hidden;
        public Visibility UpdaterButtonVisible
        {
            get
            {
                return updaterButtonVisible;
            }
            set
            {
                updaterButtonVisible = value;
                OnPropertyChanged(nameof(UpdaterButtonVisible));
            }
        }



        public void UncheckButtons()
        {
            GenericNotesChecked = false;
            ChecklistNotesChecked = false;
            FlexiChecked = false;
        }

        public ICommand FeedbackCommand { get; set; } = null!;

        public ICommand HelpCommand { get; set; } = null!;

        public ICommand SettingsCommand { get; set; }

        public ICommand GoBackCommand { get; set; }
        public ICommand GoForwardCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ICommand OpenUpdaterCommand { get; set; }


        private void UpdateByNavigationContext()
        {
            if (_navigationService.NavigationContext == NavContext.LOGGER ||
                _navigationService.NavigationContext == NavContext.POSTIT ||
                _navigationService.NavigationContext == NavContext.VIEWER ||
                _navigationService.NavigationContext == NavContext.EDITOR ||
                _navigationService.NavigationContext == NavContext.REPORTER_DASHBOARD)
            {
                MenuActivated = false;
            }
            else
                MenuActivated = true;
        }

        internal void SetGenericNotesChecked(bool isChecked)
        {
            GenericNotesChecked = isChecked;
        }

        internal void SetChecklistNotesChecked(bool isChecked)
        {
            ChecklistNotesChecked = isChecked;
        }
    }
}
