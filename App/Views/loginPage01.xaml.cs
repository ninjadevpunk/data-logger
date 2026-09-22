using Data_Logger_1._3.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for loginPage01.xaml
    /// </summary>
    public partial class loginPage01 : Page
    {
        public loginPage01(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            DataContext = loginViewModel;

            loginViewModel.RequestClose += () => Window.GetWindow(this)?.Close();

        }
    }
}
