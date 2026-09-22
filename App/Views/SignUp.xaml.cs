using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using System.Windows;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : DLSWindow
    {
        public SignUp()
        {
            InitializeComponent();
        }

        public SignUp(NavigationService navigationService, SignUpViewModel signUpViewModel)
        {
            InitializeComponent();
            DataContext = signUpViewModel;

            signUpViewModel.RequestClose += () => Close();
            VersionText.Text = AppVersion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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

        private void on_CLOSE_clicked(object sender, RoutedEventArgs e)
        {
            // Close the entire application
            Application.Current.Shutdown();
        }
    }
}
