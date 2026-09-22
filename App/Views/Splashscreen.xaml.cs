using System.Windows;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for Splashscreen.xaml
    /// </summary>
    public partial class Splashscreen : DLSWindow
    {
        public Splashscreen()
        {
            InitializeComponent();
            VersionText.Text = AppVersion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the new left position to center the window in the imaginary 3rd column
            double fourthColumnWidth = SystemParameters.PrimaryScreenWidth / 4.0;
            double windowWidth = this.Width;
            double newLeft = (3 * fourthColumnWidth) - windowWidth;

            // Calculate the new top position to center the window vertically
            double windowHeight = this.Height;
            double newTop = (SystemParameters.PrimaryScreenHeight - windowHeight) / 2.0;

            // Set the new left and top positions
            this.Left = newLeft;
            this.Top = newTop;

        }
    }
}
