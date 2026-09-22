using System.Windows.Controls;

namespace Data_Logger_1._3.Views.LogPages
{
    /// <summary>
    /// Interaction logic for CreateCheckListPage.xaml
    /// </summary>
    public partial class CreateCheckListPage : Page
    {
        public string Created { get; set; } = date();

        public CreateCheckListPage()
        {
            InitializeComponent();

            this.text_DATE.Text = "Created " + Created + ". Last modified " + Created;
        }

        public static string date()
        {
            return DateTime.Now.ToString("d MMMM yyyy HH:mm");
        }

        private void on_AddItem_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.text_DATE.Text = "Created " + Created + ". Last modified " + date();
        }
    }
}
