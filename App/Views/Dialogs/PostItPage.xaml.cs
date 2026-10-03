using Data_Logger_1._3.ViewModels.Dialogs;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PostItPage.xaml
    /// </summary>
    internal partial class PostItPage : Page
    {
        private readonly PostItViewModel _postItViewModel;

        public PostItPage(PostItViewModel postItViewModel)
        {
            InitializeComponent();
            _postItViewModel = postItViewModel;
            DataContext = _postItViewModel;
            _postItViewModel.ActiveEditor = inputText_ERROR;
        }



        private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.tab_POSTIT.SelectedIndex)
            {
                case 0:
                    _postItViewModel.ActiveEditor = inputText_ERROR;
                    break;
                case 1:
                    _postItViewModel.ActiveEditor = inputText_SOLUTION;
                    break;
                case 2:
                    _postItViewModel.ActiveEditor = inputText_SUGGESTION;
                    break;
                case 3:
                    _postItViewModel.ActiveEditor = inputText_COMMENT;
                    break;
            }
        }

    }
}
