using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for MENURadioButton.xaml
    /// </summary>
    public partial class MENURadioButton : UserControl
    {
        public MENURadioButton()
        {
            InitializeComponent();
        }



        #region Dependency Properties


        // When the menu item is selected
        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }

        public static readonly DependencyProperty CheckedProperty =
            DependencyProperty.Register("Checked", typeof(bool), typeof(MENURadioButton), new PropertyMetadata(false));



        // The group the MENU item belongs to
        public string Crew
        {
            get { return (string)GetValue(CrewProperty); }
            set { SetValue(CrewProperty, value); }
        }

        public static readonly DependencyProperty CrewProperty =
            DependencyProperty.Register("Crew", typeof(string), typeof(MENURadioButton));


        // The log's MAIN category
        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(MENURadioButton));


        // Icon on the menu button
        public Style MENUIcon
        {
            get { return (Style)GetValue(MENUIconProperty); }
            set { SetValue(MENUIconProperty, value); }
        }

        public static readonly DependencyProperty MENUIconProperty =
            DependencyProperty.Register("MENUIcon", typeof(Style), typeof(MENURadioButton));




        #endregion



        #region Event Handlers


        private void on_item_CHECKED(object sender, RoutedEventArgs e)
        {
            var ev = new RoutedEventArgs(RadioCheckEvent);
            RaiseEvent(ev);
        }

        private void on_item_UNCHECKED(object sender, RoutedEventArgs e)
        {
            var ev = new RoutedEventArgs(RadioUnCheckEvent);
            RaiseEvent(ev);
        }

        #endregion






        #region Routed Events

        public event RoutedEventHandler RadioCheck
        {
            add { AddHandler(RadioCheckEvent, value); }
            remove { RemoveHandler(RadioCheckEvent, value); }
        }

        public static readonly RoutedEvent RadioCheckEvent = EventManager.RegisterRoutedEvent(
            "RadioCheck",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MENURadioButton));

        public event RoutedEventHandler RadioUnCheck
        {
            add { AddHandler(RadioUnCheckEvent, value); }
            remove { RemoveHandler(RadioUnCheckEvent, value); }
        }

        public static readonly RoutedEvent RadioUnCheckEvent = EventManager.RegisterRoutedEvent(
            "RadioUnCheck",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MENURadioButton));



        #endregion
    }
}
