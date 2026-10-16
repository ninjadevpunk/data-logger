using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for SubMENURadioButton.xaml
    /// </summary>
    public partial class SubMENURadioButton : UserControl
    {
        public SubMENURadioButton()
        {
            InitializeComponent();
        }

        #region Dependency Properties


        // When the menu item is selected
        public bool SubChecked
        {
            get { return (bool)GetValue(SubCheckedProperty); }
            set { SetValue(SubCheckedProperty, value); }
        }

        public static readonly DependencyProperty SubCheckedProperty =
            DependencyProperty.Register("SubChecked", typeof(bool), typeof(SubMENURadioButton));



        // The group the MENU item belongs to
        public string SubCrew
        {
            get { return (string)GetValue(SubCrewProperty); }
            set { SetValue(SubCrewProperty, value); }
        }

        public static readonly DependencyProperty SubCrewProperty =
            DependencyProperty.Register("SubCrew", typeof(string), typeof(SubMENURadioButton));


        // The log's MAIN category
        public string SubCategory
        {
            get { return (string)GetValue(SubCategoryProperty); }
            set { SetValue(SubCategoryProperty, value); }
        }

        public static readonly DependencyProperty SubCategoryProperty =
            DependencyProperty.Register("SubCategory", typeof(string), typeof(SubMENURadioButton));


        // Icon on the menu button
        public Style SubMENUIcon
        {
            get { return (Style)GetValue(SubMENUIconProperty); }
            set { SetValue(SubMENUIconProperty, value); }
        }

        public static readonly DependencyProperty SubMENUIconProperty =
            DependencyProperty.Register("SubMENUIcon", typeof(Style), typeof(SubMENURadioButton));




        #endregion













        #region Event Handlers


        private void on_item_SubCHECKED(object sender, RoutedEventArgs e)
        {
            var ev = new RoutedEventArgs(SubRadioCheckEvent);
            RaiseEvent(ev);
        }

        private void on_item_SubUNCHECKED(object sender, RoutedEventArgs e)
        {
            var ev = new RoutedEventArgs(SubRadioUnCheckEvent);
            RaiseEvent(ev);
        }

        #endregion


        #region Routed Events

        public event RoutedEventHandler SubRadioCheck
        {
            add { AddHandler(SubRadioCheckEvent, value); }
            remove { RemoveHandler(SubRadioCheckEvent, value); }
        }

        public static readonly RoutedEvent SubRadioCheckEvent = EventManager.RegisterRoutedEvent(
            "RadioCheck",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SubMENURadioButton));

        public event RoutedEventHandler SubRadioUnCheck
        {
            add { AddHandler(SubRadioUnCheckEvent, value); }
            remove { RemoveHandler(SubRadioUnCheckEvent, value); }
        }

        public static readonly RoutedEvent SubRadioUnCheckEvent = EventManager.RegisterRoutedEvent(
            "RadioUnCheck",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SubMENURadioButton));



        #endregion
    }
}
