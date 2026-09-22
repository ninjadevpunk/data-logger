using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views.ReportPages
{
    /// <summary>
    /// Interaction logic for ReporterDashboard.xaml
    /// </summary>
    public partial class ReporterDashboard : Page
    {
        private readonly ReportDeskViewModel _reportDeskViewModel;

        public ReporterDashboard()
        {
            InitializeComponent();
        }

        public ReporterDashboard(ReportDeskViewModel reportDeskViewModel)
        {
            InitializeComponent();

            DataContext = reportDeskViewModel;
            _reportDeskViewModel = reportDeskViewModel;
        }

        private async void on_Application_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            await _reportDeskViewModel.InitialiseProjectsAsync();
            await _reportDeskViewModel.UpdateLogsListAsync();
        }

        private async void on_Project_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            await _reportDeskViewModel.UpdateLogsListAsync();
        }
    }
}
