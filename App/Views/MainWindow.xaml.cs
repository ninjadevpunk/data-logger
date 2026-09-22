using Data_Logger_1._3.Components;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DLSWindow
    {
        private readonly NavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow(MainWindowViewModel mainWindowViewModel, NavigationService navigationService)
        {
            InitializeComponent();

            // Create a DoubleAnimation to animate the height of the radiobutton.
            DoubleAnimation startAnimate = new(), endAnimate = new(), n1animation = new(), n2animation = new();
            startAnimate.From = 0;
            startAnimate.To = 180;
            startAnimate.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            n1animation.From = 0;
            n1animation.To = 180;
            n1animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            endAnimate.From = 180;
            endAnimate.To = 0;
            endAnimate.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            n2animation.From = 180;
            n2animation.To = 0;
            n2animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            HeightAnimationStart(startAnimate, this.STACK_CODING.Name, this.radio_CODING, this.STACK_CODING);
            HeightAnimationEnd(endAnimate, this.STACK_CODING.Name, this.radio_CODING, this.STACK_CODING);
            HeightAnimationStart(n1animation, this.STACK_NOTES.Name, this.radio_NOTES, this.STACK_NOTES);
            HeightAnimationEnd(n2animation, this.STACK_NOTES.Name, this.radio_NOTES, this.STACK_NOTES);

            DataContext = mainWindowViewModel;
            _navigationService = navigationService;
            _mainWindowViewModel = mainWindowViewModel;
        }

        # region Animations



        public void HeightAnimationStart(DependencyObject d, string name, MENURadioButton rad, StackPanel stack)
        {
            // Configure the animation to target the radiobutton's height property.
            Storyboard.SetTargetName((DoubleAnimation)d, name);
            Storyboard.SetTargetProperty((DoubleAnimation)d, new PropertyPath(StackPanel.HeightProperty));

            // Create a storyboard to contain the animation.
            var story1 = new Storyboard();
            story1.Children.Add((DoubleAnimation)d);

            // Animate the radiobutton height when it's clicked.
            rad.RadioCheck += delegate (object sender, RoutedEventArgs args)
            {
                if (rad.Checked)
                    story1.Begin(stack);
            };
        }

        public void HeightAnimationEnd(DependencyObject d, string name, MENURadioButton rad, StackPanel stack)
        {
            // Configure the animation to target the radiobutton's height property.
            Storyboard.SetTargetName((DoubleAnimation)d, name);
            Storyboard.SetTargetProperty((DoubleAnimation)d, new PropertyPath(StackPanel.HeightProperty));

            // Create a storyboard to contain the animation.
            var story1 = new Storyboard();
            story1.Children.Add((DoubleAnimation)d);

            // Animate the radiobutton height when it's clicked.
            rad.RadioUnCheck += delegate (object sender, RoutedEventArgs args)
            {
                if (!rad.Checked)
                    story1.Begin(stack);
            };
        }




        #endregion

        private void on_MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the new left position to center the window horizontally
            double windowWidth = this.Width;
            double newLeft = (SystemParameters.PrimaryScreenWidth - windowWidth) / 2.0;

            // Calculate the new top position to center the window vertically
            double windowHeight = this.Height;
            double newTop = (SystemParameters.PrimaryScreenHeight - windowHeight) / 2.0;

            // Set the new left and top positions
            this.Left = newLeft;
            this.Top = newTop;
        }













        private void on_MINIMISE_clicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void on_MAXIMISE_clicked(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // If the window is currently maximized, restore it
                WindowState = WindowState.Normal;
            }
            else
            {
                // If the window is not maximized, maximize it
                WindowState = WindowState.Maximized;
            }
        }

        private void on_CLOSE_clicked(object sender, RoutedEventArgs e)
        {
            // Close the entire application
            Application.Current.Shutdown();
        }

        private void on_customTitleBar_pressed(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void on_Qt_checked(object sender, RoutedEventArgs e)
        {
            if (_mainWindowViewModel.CodingQtChecked)
                await _navigationService.NavigateToLogCachePage(CacheMaster.CacheContext.Qt);
        }

        private async void on_AndroidStudio_checked(object sender, RoutedEventArgs e)
        {
            if (_mainWindowViewModel.CodingAndroidChecked)
                await _navigationService.NavigateToLogCachePage(CacheMaster.CacheContext.AndroidStudio);
        }

        private async void on_GenericCoding_checked(object sender, RoutedEventArgs e)
        {
            if (_mainWindowViewModel.CodingGenericChecked)
                await _navigationService.NavigateToLogCachePage(CacheMaster.CacheContext.Coding);
        }

        private async void on_NOTES_checked(object sender, RoutedEventArgs e)
        {
            if(_mainWindowViewModel.NotesChecked)
                await _navigationService.NavigateToNotesDashboard();
        }

        private async void on_GenericNotes_checked(object sender, RoutedEventArgs e)
        {
            if(_mainWindowViewModel.GenericNotesChecked)
                await _navigationService.NavigateToCreateNotesPage();
        }
    }
}