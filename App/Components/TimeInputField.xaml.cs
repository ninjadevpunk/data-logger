using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for TimeInputField.xaml
    /// </summary>
    public partial class TimeInputField : UserControl
    {
        public TimeInputField()
        {
            InitializeComponent();
        }


        #region Dependency Properties




        public string Hours
        {
            get { return (string)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(string), typeof(TimeInputField),


                new PropertyMetadata("00")
                );



        public string Minutes
        {
            get { return (string)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(string), typeof(TimeInputField),


                new PropertyMetadata("00")
                );



        public string Seconds
        {
            get { return (string)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds", typeof(string), typeof(TimeInputField),


                new PropertyMetadata("00")
                );



        public string Milliseconds
        {
            get { return (string)GetValue(MillisecondsProperty); }
            set { SetValue(MillisecondsProperty, value); }
        }

        public static readonly DependencyProperty MillisecondsProperty =
            DependencyProperty.Register("Milliseconds", typeof(string), typeof(TimeInputField),


                new PropertyMetadata("000")
                );

        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TimeInputField));




        #endregion





        #region Member Functions


        public void setTime()
        {
            Time = DateTime.Now;

            Hours = DateTime.Now.Hour.ToString();
            Minutes = DateTime.Now.Minute.ToString();
            Seconds = DateTime.Now.Second.ToString();
            Milliseconds = DateTime.Now.Millisecond.ToString();
        }





        #endregion





        #region Event Handlers





        public void updateTime(object sender, RoutedEventArgs e)
        {
            if (this.spinner_MILLISECONDS is null)
                return;

            if (this.spinner_SECONDS is null)
                return;

            if (this.spinner_MINUTES is null)
                return;

            if (this.spinner_HOURS is null)
                return;

            try
            {
                Milliseconds = NumberFormatter(this.spinner_MILLISECONDS.Digits, Milliseconds);

                Seconds = NumberFormatter(this.spinner_SECONDS.Digits, Seconds);

                Minutes = NumberFormatter(this.spinner_MINUTES.Digits, Minutes);

                Hours = NumberFormatter(this.spinner_HOURS.Digits, Hours);


                this.Time = DateTime.Parse(
                Hours + ":" +
                Minutes + ":" +
                Seconds + "." +
                Milliseconds);
                
                var ev = new RoutedEventArgs(TimeChangedEvent);
                RaiseEvent(ev);
            }
            catch (Exception ex)
            {
                setTime();
                Debug.WriteLine($"Exception found near updateTime(): {ex.Message}");


                // TODO

            }

        }

        public string NumberFormatter(int realLength, string currentNumber)
        {
            return currentNumber.PadLeft(realLength, '0');
        }


        #endregion






        #region Routed Events






        public event RoutedEventHandler TimeChanged
        {
            add { AddHandler(TimeChangedEvent, value); }
            remove { RemoveHandler(TimeChangedEvent, value); }
        }

        public static readonly RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent(
            "TimeChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TimeInputField));




        #endregion

    }
}
