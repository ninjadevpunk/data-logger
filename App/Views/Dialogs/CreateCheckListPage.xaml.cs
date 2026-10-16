using Data_Logger_1._3.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views.LogPages
{
    /// <summary>
    /// Interaction logic for CreateCheckListPage.xaml
    /// </summary>
    internal partial class CreateCheckListPage : Page
    {
        public string CreationDate { get; } = Extensions.NoteDateTimeNow();
        public static string ModifiedDate => Extensions.NoteDateTimeNow();

        public CreateCheckListPage()
        {
            InitializeComponent();

            UpdateDates();
        }

        public void UpdateDates()
        {
            text_DATE.Text = Extensions.CreatedAndModifiedDate(CreationDate, ModifiedDate);
        }

        private void On_AddItem_clicked(object sender, RoutedEventArgs e)
        {
            UpdateDates();
        }
    }
}
