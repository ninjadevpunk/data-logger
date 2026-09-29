using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Data_Logger_1._3.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PostItPage.xaml
    /// </summary>
    public partial class PostItPage : Page
    {
        private readonly PostItViewModel? _postItViewModel;
        private readonly EF_PostItViewModel? _efPostItViewModel;

        internal PostItPage(PostItViewModel createPostItViewModel)
        {
            InitializeComponent();

            _postItViewModel = createPostItViewModel;
            DataContext = _postItViewModel;
            _postItViewModel.ActiveEditor = this.inputText_ERROR;


        }

        public PostItPage(EF_PostItViewModel efPostItViewModel)
        {
            InitializeComponent();

            _efPostItViewModel = efPostItViewModel;
            DataContext = _efPostItViewModel;
            _efPostItViewModel.ActiveEditor = this.inputText_ERROR;


        }

        public PostItPage()
        {
            
        }

        public static string RtfStringToPlainText(string rtfString)
        {
            Xceed.Wpf.Toolkit.RichTextBox tempRtBox = new Xceed.Wpf.Toolkit.RichTextBox(new FlowDocument());

            
            tempRtBox.TextFormatter = new Xceed.Wpf.Toolkit.RtfFormatter();
            tempRtBox.Text = rtfString;

            tempRtBox.TextFormatter = new Xceed.Wpf.Toolkit.PlainTextFormatter();
            return tempRtBox.Text;
        }

        private void OnTextChanged(object sender, EventArgs e, Xceed.Wpf.Toolkit.RichTextBox inputText, Action<string> setDisplayProperty)
        {
            try
            {
                var plainText = RtfStringToPlainText(inputText.Text);

                setDisplayProperty(plainText);
            }
            catch (Exception)
            {
                // 
            }
        }


        private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.tab_POSTIT.SelectedIndex)
            {
                case 0:
                    {
                        if(_postItViewModel != null)
                            _postItViewModel.ActiveEditor = this.inputText_ERROR;
                        else if(_efPostItViewModel != null)
                            _efPostItViewModel.ActiveEditor = this.inputText_ERROR;

                        break;
                    }
                case 1:
                    {
                        if (_postItViewModel != null)
                            _postItViewModel.ActiveEditor = this.inputText_SOLUTION;
                        else if (_efPostItViewModel != null)
                            _efPostItViewModel.ActiveEditor = this.inputText_SOLUTION;

                        break;
                    }
                case 2:
                    {
                        if (_postItViewModel != null)
                            _postItViewModel.ActiveEditor = this.inputText_SUGGESTION;
                        else if (_efPostItViewModel != null)
                            _efPostItViewModel.ActiveEditor = this.inputText_SUGGESTION;

                        break;
                    }
                case 3:
                    {
                        if (_postItViewModel != null)
                            _postItViewModel.ActiveEditor = this.inputText_COMMENT;
                        else if (_efPostItViewModel != null)
                            _efPostItViewModel.ActiveEditor = this.inputText_COMMENT;

                        break;
                    }
            }
        }
    }
}
