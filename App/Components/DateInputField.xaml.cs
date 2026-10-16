using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for DateInputField.xaml
    /// </summary>
    public partial class DateInputField : UserControl
    {
        public DateInputField()
        {
            InitializeComponent();
            this.datePicker_DATE.SelectedDateFormat = DatePickerFormat.Long;

        }

        #region Dependency Properties


        public DateTime Date
        {
            get { return (DateTime)GetValue(TheDateProperty); }
            set { SetValue(TheDateProperty, value); }
        }

        public static readonly DependencyProperty TheDateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DateInputField),

                new PropertyMetadata(DateTime.Now)
                );



        //public string PlaceholderText
        //{
        //    get { return (string)GetValue(PlaceholderTextProperty); }
        //    set { SetValue(PlaceholderTextProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PlaceholderTextProperty =
        //    DependencyProperty.Register("PlaceholderText", typeof(string), typeof(DateInputField));





        #endregion











        #region Member Functions


        public void setDate()
        {
            this.datePicker_DATE.SelectedDate = DateTime.Now;
        }


        #endregion










        #region Event Handlers 
        private void on_DATE_changed(object sender, SelectionChangedEventArgs e)
        {
            var ev = new RoutedEventArgs(DateChangedEvent);
            RaiseEvent(ev);
        }


        #endregion






        #region Routed Events


        public event RoutedEventHandler DateChanged
        {
            add { AddHandler(DateChangedEvent, value); }
            remove { RemoveHandler(DateChangedEvent, value); }
        }

        public static readonly RoutedEvent DateChangedEvent = EventManager.RegisterRoutedEvent(
            "DateChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(DateInputField));




        #endregion








    }
}
