using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for PlainInputField.xaml
    /// </summary>
    public partial class PlainInputField : UserControl
    {

        public PlainInputField()
        {
            InitializeComponent();
            UpdatePlaceholderText();
        }

        #region Dependency Properties


        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(PlainInputField));



        public string DynamicPlaceholderText
        {
            get { return (string)GetValue(DynamicPlaceholderTextProperty); }
            set { SetValue(DynamicPlaceholderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DynamicPlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DynamicPlaceholderTextProperty =
            DependencyProperty.Register("DynamicPlaceholderText", typeof(string), typeof(PlainInputField));



        public string UserInput
        {
            get { return (string)GetValue(UserInputProperty); }
            set { SetValue(UserInputProperty, value); }
        }

        public static readonly DependencyProperty UserInputProperty =
            DependencyProperty.Register("UserInput", typeof(string), typeof(PlainInputField));



        // Text Alignment
        public string Aligned
        {
            get { return (string)GetValue(AlignedProperty); }
            set { SetValue(AlignedProperty, value); }
        }

        public static readonly DependencyProperty AlignedProperty =
            DependencyProperty.Register("Aligned", typeof(string), typeof(PlainInputField));


        // TextBox Style
        public Style UserTextBoxStyle
        {
            get { return (Style)GetValue(UserTextBoxStyleProperty); }
            set { SetValue(UserTextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBoxStyleProperty =
            DependencyProperty.Register("UserTextBoxStyle", typeof(Style), typeof(PlainInputField));

        // TextBlock Style
        public Style UserTextBlockStyle
        {
            get { return (Style)GetValue(UserTextBlockStyleProperty); }
            set { SetValue(UserTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBlockStyleProperty =
            DependencyProperty.Register("UserTextBlockStyle", typeof(Style), typeof(PlainInputField));

        #endregion




        #region Member Functions


        private void on_inputText_lostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePlaceholderText();
        }


        public void showPlaceholderText()
        {
            DynamicPlaceholderText = PlaceholderText;
        }


        public void UpdatePlaceholderText()
        {
            DynamicPlaceholderText = UserInput is not null && UserInput.Equals(string.Empty) ? PlaceholderText : string.Empty;
        }



        #endregion








        #region Event Handlers


        private void on_inputText_text_changed(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderText();

            var ev = new RoutedEventArgs(TextChangedEvent);
            RaiseEvent(ev);
        }








        #endregion




        #region Routed Events


        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
            "TextChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PlainInputField));








        #endregion

    }
}
