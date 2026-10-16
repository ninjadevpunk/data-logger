using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for ComboBoxField.xaml
    /// </summary>
    public partial class ComboBoxField : UserControl
    {

        public ComboBoxField()
        {

            InitializeComponent();

        }





        #region Dependency Properties





        /// <summary>
        /// Checklist in the combo box.
        /// </summary>
        public IEnumerable<string> Items
        {
            get { return (IEnumerable<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(ComboBoxField));


        /// <summary>
        /// For enabling and disabling editing the control.
        /// </summary>
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(ComboBoxField), new PropertyMetadata(true));

        // User's selection
        public string UserSelection
        {
            get { return (string)GetValue(UserSelectionProperty); }
            set { SetValue(UserSelectionProperty, value); }
        }

        public static readonly DependencyProperty UserSelectionProperty =
            DependencyProperty.Register("UserSelection", typeof(string), typeof(ComboBoxField));


        // Style for the TextBox in the ComboBox
        public Style UserTextBoxStyle
        {
            get { return (Style)GetValue(UserTextBoxStyleProperty); }
            set { SetValue(UserTextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBoxStyleProperty =
            DependencyProperty.Register("UserTextBoxStyle", typeof(Style), typeof(ComboBoxField));


        // TextBlock Style
        public Style UserTextBlockStyle
        {
            get { return (Style)GetValue(UserTextBlockStyleProperty); }
            set { SetValue(UserTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBlockStyleProperty =
            DependencyProperty.Register("UserTextBlockStyle", typeof(Style), typeof(ComboBoxField));





        #endregion







        #region Member Functions






        #endregion







        #region Event Handlers






        private void on_ComboBox_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            var ev = new RoutedEventArgs(ComboBoxSelectionChangedEvent, this); 
            RaiseEvent(ev);
        }



        #endregion






        #region Routed Events






        public event RoutedEventHandler ComboBoxSelectionChanged
        {
            add { AddHandler(ComboBoxSelectionChangedEvent, value); }
            remove { RemoveHandler(ComboBoxSelectionChangedEvent, value); }
        }

        public static readonly RoutedEvent ComboBoxSelectionChangedEvent = EventManager.RegisterRoutedEvent(
            "ComboBoxSelectionChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ComboBoxField));



        #endregion


    }
}
