namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for Splashscreen.xaml
    /// </summary>
    internal partial class Splashscreen : DLSWindow
    {
        public Splashscreen()
        {
            InitializeComponent();
            VersionText.Text = AppVersion;
        }

    }
}
