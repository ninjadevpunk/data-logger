using Data_Logger_1._3.ViewModels.Dialogs;
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

        public PostItPage(PostItViewModel createPostItViewModel)
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

        private void on_Error_Text_Changed(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(sender, e, inputText_ERROR, text =>
            {
                if (_postItViewModel != null)
                    _postItViewModel.Display_Error= text;
                else if (_efPostItViewModel != null)
                    _efPostItViewModel.Display_Error= text;
            });
        }

        private void on_Solution_Text_Changed(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(sender, e, inputText_SOLUTION, text =>
            {
                if (_postItViewModel != null)
                    _postItViewModel.Display_Solution= text;
                else if (_efPostItViewModel != null)
                    _efPostItViewModel.Display_Solution= text;
            });
        }

        private void on_Suggestion_Text_Changed(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(sender, e, inputText_SUGGESTION, text =>
            {
                if(_postItViewModel != null)
                    _postItViewModel.Display_Suggestion = text;
                else if(_efPostItViewModel != null)
                    _efPostItViewModel.Display_Suggestion = text;
            });
        }

        private void on_Comment_Text_Changed(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(sender, e, inputText_COMMENT, text =>
            {
                if (_postItViewModel != null)
                    _postItViewModel.Display_Comment= text;
                else if (_efPostItViewModel != null)
                    _efPostItViewModel.Display_Comment= text;
            });
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
