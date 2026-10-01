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

        internal SignUp(SignUpViewModel signUpViewModel)
        {
            InitializeComponent();
            DataContext = signUpViewModel;

            signUpViewModel.RequestClose += () => Close();
            VersionText.Text = AppVersion;
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
