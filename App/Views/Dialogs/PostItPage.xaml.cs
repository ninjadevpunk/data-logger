using Data_Logger_1._3.ViewModels.Dialogs;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Data_Logger_1._3.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PostItPage.xaml
    /// </summary>
    public partial class PostItPage : Page
    {
        private readonly PostItViewModel _postItViewModel;

        internal PostItPage(PostItViewModel postItViewModel)
        {
            InitializeComponent();
            _postItViewModel = postItViewModel;
        }



        private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.tab_POSTIT.SelectedIndex)
            {
                case 0:
                    _postItViewModel.ActiveEditor = this.inputText_ERROR;
                    break;
                case 1:
                    _postItViewModel.ActiveEditor = this.inputText_SOLUTION;
                    break;
                case 2:
                    _postItViewModel.ActiveEditor = this.inputText_SUGGESTION;
                    break;
                case 3:
                    _postItViewModel.ActiveEditor = this.inputText_COMMENT;
                    break;
            }
        }

    }
}
