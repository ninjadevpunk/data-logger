using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for ComboBoxInputField.xaml
    /// </summary>
    public partial class ComboBoxInputField : UserControl
    {

        public ComboBoxInputField()
        {

            InitializeComponent();
            UpdatePlaceholderText();
        }





        #region Dependency Properties


        /// <summary>
        /// Static placeholder text. NOT to be modified.
        /// </summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(ComboBoxInputField));

        public string DynamicPlaceholderText
        {
            get { return (string)GetValue(DynamicPlaceholderTextProperty); }
            set { SetValue(DynamicPlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty DynamicPlaceholderTextProperty =
            DependencyProperty.Register("DynamicPlaceholderText", typeof(string), typeof(ComboBoxInputField));


        // User's input
        public string UserComboInput
        {
            get { return (string)GetValue(UserComboInputProperty); }
            set { SetValue(UserComboInputProperty, value); }
        }

        public static readonly DependencyProperty UserComboInputProperty =
            DependencyProperty.Register("UserComboInput", typeof(string), typeof(ComboBoxInputField));

        // Checklist in the combo box
        public IEnumerable<string> Items
        {
            get { return (IEnumerable<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(ComboBoxInputField));


        // For enabling and disabling editing the control. Will allow it to be a normal combo box if false.
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(ComboBoxInputField), new PropertyMetadata(true));



        // Style for the TextBox in the ComboBox
        public Style UserTextBoxStyle
        {
            get { return (Style)GetValue(UserTextBoxStyleProperty); }
            set { SetValue(UserTextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBoxStyleProperty =
            DependencyProperty.Register("UserTextBoxStyle", typeof(Style), typeof(ComboBoxInputField));


        // TextBlock Style
        public Style UserTextBlockStyle
        {
            get { return (Style)GetValue(UserTextBlockStyleProperty); }
            set { SetValue(UserTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBlockStyleProperty =
            DependencyProperty.Register("UserTextBlockStyle", typeof(Style), typeof(ComboBoxInputField));





        #endregion







        #region Member Functions



        private void on_PLACEHOLDERTEXT_loaded(object sender, RoutedEventArgs e)
        {

            // If the combobox is not Editable, show no placeholder text.
            //if (!Enabled)
            //{
            //    text_PLACEHOLDERTEXT.Visibility = Visibility.Hidden;
            //    InputChanged = false;

            //    return;
            //}


        }

        private void on_EditableTextBox_lostfocus(object sender, RoutedEventArgs e)
        {
            UpdatePlaceholderText();
        }


        public void UpdatePlaceholderText()
        {
            DynamicPlaceholderText = UserComboInput is not null && UserComboInput.Equals(string.Empty) ? PlaceholderText : string.Empty;
        }





        #endregion







        #region Event Handlers



        private void on_EditableTextBox_changed(object sender, TextChangedEventArgs e)
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
            typeof(ComboBoxInputField));








        #endregion


    }
}
