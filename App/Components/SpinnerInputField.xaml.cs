using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for SpinnerInputField.xaml
    /// </summary>
    public partial class SpinnerInputField : UserControl
    {
        public SpinnerInputField()
        {
            InitializeComponent();

            buttonCheck();
            SetLeader();

            if (Number != null)
            {

                if (IsInteger)
                    Number = Formatter("0");
                else
                    Number = Formatter("0,0");
            }


        }

        #region Dependency Properties



        // Specify whether the spinner control is for integers or doubles. They are integers by default.
        public bool IsInteger
        {
            get { return (bool)GetValue(IsIntegerProperty); }
            set { SetValue(IsIntegerProperty, value); }
        }

        public static readonly DependencyProperty IsIntegerProperty =
            DependencyProperty.Register("IsInteger", typeof(bool), typeof(SpinnerInputField),


                new PropertyMetadata(true)
                );

        // How many digits are in the control?
        // Be careful of double values. This control accepts both integers and doubles so doubles include the decimal points.
        public int Digits
        {
            get { return (int)GetValue(DigitsProperty); }
            set { SetValue(DigitsProperty, value); }
        }

        public static readonly DependencyProperty DigitsProperty =
            DependencyProperty.Register("Digits", typeof(int), typeof(SpinnerInputField));

        // How main leading zeroes are in the number to fill the digit deficit? Empty string by default.
        public string Leader
        {
            get { return (string)GetValue(LeaderProperty); }
            set { SetValue(LeaderProperty, value); }
        }

        public static readonly DependencyProperty LeaderProperty =
            DependencyProperty.Register("Leader", typeof(string), typeof(SpinnerInputField),


                new PropertyMetadata("")
                );

        // What is the actual number? String "0" by default.
        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(string), typeof(SpinnerInputField),


                new PropertyMetadata("0")
                );

        // The smallest number this control can hold. 0 by default.
        public int MinimumValue
        {
            get { return (int)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register("MinimumValue", typeof(int), typeof(SpinnerInputField),


                new PropertyMetadata(0)
                );



        // The largest number this control can hold. 100 by default.
        public int MaximumValue
        {
            get { return (int)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register("MaximumValue", typeof(int), typeof(SpinnerInputField),


                new PropertyMetadata(100)
                );


        #endregion



        #region Member Functions



        public void SetLeader()
        {
            if (Leader is null)
                return;

            if (Digits == 5)
            {
                Leader = "0000";
            }
            else if (Digits == 4)
            {
                Leader = "000";
            }
            else if (Digits == 3)
            {
                Leader = "00";
            }
            else if (Digits == 2)
            {
                Leader = "0";
            }
            else
            {
                Leader = "";
            }

        }

        // When up button is clicked, increment and set the leading zeroes
        private void on_INCREMENT_click(object sender, RoutedEventArgs e)
        {
            SetLeader();
            increment();
            buttonCheck();
        }


        // When down button is clicked, decrement and set the leading zeroes
        private void on_DECREMENT_click(object sender, RoutedEventArgs e)
        {
            SetLeader();
            decrement();
            buttonCheck();
        }

        // When the spinner text changes, ensure value is in range
        private void on_SPINNER_textChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInteger)
            {
                try
                {
                    if (int.Parse(this.Number) > MaximumValue)
                    {
                        this.Number = $"{MaximumValue}";
                    }

                    if (int.Parse(this.Number) < MinimumValue)
                    {
                        this.Number = $"{MinimumValue}";
                    }
                }
                catch (Exception)
                {

                    // TODO Send to Firebase
                }
            }
            else
            {
                try
                {
                    if (double.Parse(this.Number) > MaximumValue)
                    {
                        this.Number = $"{MaximumValue}";
                    }

                    if (double.Parse(this.Number) < MinimumValue)
                    {
                        this.Number = $"{MinimumValue}";
                    }
                }
                catch (Exception)
                {

                    // TODO Send to Firebase
                }
            }

            buttonCheck();

            var ev = new RoutedEventArgs(NumberChangedEvent);
            RaiseEvent(ev);
        }


        // When spinner text has lost focus, call validator
        private void on_SPINNER_lostFocus(object sender, RoutedEventArgs e)
        {
            validate();

            if (IsInteger)
            {
                if (this.spinner_NUMBERS.Text == "" || this.spinner_NUMBERS.Text == null)
                {

                    this.spinner_NUMBERS.Text = Formatter("0");
                }
            }
            else
            {
                if (this.spinner_NUMBERS.Text == "" || this.spinner_NUMBERS.Text == null)
                {
                    var temp = "";


                    for (int i = 0; i < Digits; i++)
                    {
                        temp += Leader.Substring(0, 1);
                    }

                    this.spinner_NUMBERS.Text = Formatter("0,0");
                }
            }

            buttonCheck(); 
            
            var ev = new RoutedEventArgs(NumberChangedEvent);
            RaiseEvent(ev);
        }

        // When the scroll wheel is rolled over the spinner, increment/decrement
        private void on_SPINNER_rotate(object sender, MouseWheelEventArgs e)
        {




            try
            {


                // e.Handled ensures that the ScrollViewer doesn't scroll when the mousewheel rotates over the spinner
                e.Handled = true;

                // Ensure that the number is an integer first. Then make sure that when a maximum or minimum value is reached,
                // the scrolling is unable to increment/decrement.
                if (IsInteger)
                {
                    if (e.Delta > 0 && int.Parse(this.Number) != MaximumValue)
                    {
                        SetLeader();
                        increment();

                    }
                    else if (e.Delta < 0 && int.Parse(this.Number) != MinimumValue)
                    {
                        SetLeader();
                        decrement();
                    }
                }
                else
                {
                    if (e.Delta > 0 && double.Parse(this.Number) != MaximumValue)
                    {
                        increment();

                    }
                    else if (e.Delta < 0 && double.Parse(this.Number) != MinimumValue)
                    {
                        decrement();
                    }
                }


                buttonCheck();




            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near on_SPINNER_rotate(): {ex.Message}");
            
            // TODO

            }

        }

        // Make it clear that a button can't be clicked on due to the max or min value being reached
        private void buttonCheck()
        {
            if (this.button_INCREMENT is null)
                return;

            try
            {
                if (IsInteger)
                {


                    // INCREMENT
                    if (int.Parse(Number) == this.MaximumValue)
                        this.button_INCREMENT.IsEnabled = false;
                    else
                        this.button_INCREMENT.IsEnabled = true;

                    // DECREMENT
                    if (int.Parse(Number) == this.MinimumValue)
                        this.button_DECREMENT.IsEnabled = false;
                    else
                        this.button_DECREMENT.IsEnabled = true;
                }
                else
                {

                    // INCREMENT
                    if (double.Parse(Number) == this.MaximumValue)
                        this.button_INCREMENT.IsEnabled = false;
                    else
                        this.button_INCREMENT.IsEnabled = true;

                    // DECREMENT
                    if (double.Parse(Number) == this.MinimumValue)
                        this.button_DECREMENT.IsEnabled = false;
                    else
                        this.button_DECREMENT.IsEnabled = true;
                }
            }
            catch (Exception)
            {

                // Send to Firebase
            }
        }



        static string FormatAndRoundNumber(double number)
        {
            // Use a custom format specifier to limit to 3 digits before the decimal point
            // and 2 digits after the decimal point. The "F" format ensures fixed-point
            // notation, and "N" specifies the number of decimal places.
            string formattedNumber = number.ToString("F2", CultureInfo.InvariantCulture);

            // If the number has more than 5 digits, truncate the extra digits
            if (formattedNumber.Length > 7)
            {
                formattedNumber = formattedNumber.Substring(0, 5);
            }

            return formattedNumber;
        }

        // Validator
        public bool validate()
        {
            try
            {
                if (this.spinner_NUMBERS.Text == "" || this.spinner_NUMBERS.Text == null)
                {
                    if (IsInteger)
                        this.spinner_NUMBERS.Text = Formatter("0");
                    else
                        this.spinner_NUMBERS.Text = Formatter("0,0");

                    return true;
                }

                var n = double.Parse(this.spinner_NUMBERS.Text);


                if (n > MaximumValue)
                {
                    throw new InvalidOperationException($"Your number is higher than {MaximumValue}. Please enter a lower number.");
                }

                if (n < MinimumValue)
                {
                    throw new InvalidOperationException($"Your number is lower than {MinimumValue}. Please enter a lower number.");
                }


            }
            catch (FormatException fe)
            {
                MessageBox.Show($"An error has occurred on our side. This app will send feedback for you to solve the problem. We apologise for any incovenience.",
                    "Format Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                // TODO Send to database instead of console so that the bug can be fixed.
                Debug.WriteLine(fe.ToString());

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error has occurred. Please ensure you have entered valid input. This app will send feedback for you to solve the problem. " +
                    $"If the problem persists, please use the Feedback tool on the bottom of the menu panel.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                // TODO Send to database instead of console so that the bug can be fixed.

                Debug.WriteLine($"Exception found: {e.Message}");


                if (IsInteger)
                    this.spinner_NUMBERS.Text = Formatter("0");
                else
                    this.spinner_NUMBERS.Text = Formatter("0,0");

                var ev = new RoutedEventArgs(NumberChangedEvent);
                RaiseEvent(ev);

                return false;
            }

            return true;
        }




        // Number Formatter
        public void Formatter()
        {
            this.Number.PadLeft(Digits, '0');
        }

        public string Formatter(string value)
        {
            return value.PadLeft(Digits, '0');
        }

        #endregion



        #region Event Handlers


        // Increment the number
        private void increment()
        {
            try
            {
                if (IsInteger)
                {
                    var value = int.Parse(this.spinner_NUMBERS.Text);
                    ++value;

                    this.Number = Formatter(value.ToString());
                }
                else
                {
                    double value = double.Parse(this.spinner_NUMBERS.Text);
                    ++value;

                    if (value < 10d)
                        this.Number = $"{Leader}{FormatAndRoundNumber(value)}".Replace('.', ',');
                    else
                        this.Number = $"{Leader.Remove(0, 1)}{FormatAndRoundNumber(value)}".Replace('.', ',');
                }

                if (this.spinner_NUMBERS.Text.Length > Digits)
                {
                    this.spinner_NUMBERS.Text = this.spinner_NUMBERS.Text.Substring(1, Digits);
                }


                var ev = new RoutedEventArgs(IsIncrementedEvent);
                RaiseEvent(ev);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near increment(): {e.Message}");

                // Send to database


            }

            buttonCheck();
        }

        // Decrement the number
        private void decrement()
        {
            try
            {
                if (IsInteger)
                {
                    var value = int.Parse(this.spinner_NUMBERS.Text);
                    --value;

                    if (value < 10)
                        this.Number = $"{Leader}{value}";
                    else
                        this.Number = $"{Leader.Remove(0, 1)}{value}";

                    Formatter();
                }
                else
                {
                    double value = double.Parse(this.spinner_NUMBERS.Text);
                    --value;

                    if (value < 10d)
                        this.Number = $"{Leader}{FormatAndRoundNumber(value)}".Replace('.', ',');
                    else
                        this.Number = $"{Leader.Remove(0, 1)}{FormatAndRoundNumber(value)}".Replace('.', ',');
                }

                if (this.spinner_NUMBERS.Text.Length > Digits)
                {
                    this.spinner_NUMBERS.Text = this.spinner_NUMBERS.Text.Substring(1, Digits);
                }

                var ev = new RoutedEventArgs(IsDecrementedEvent);
                RaiseEvent(ev);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near decrement(): {e.Message}");

                // Send to database


            }

            buttonCheck();
        }






        #endregion



        public event RoutedEventHandler IsIncremented
        {
            add { AddHandler(IsIncrementedEvent, value); }
            remove { RemoveHandler(IsIncrementedEvent, value); }
        }

        public static readonly RoutedEvent IsIncrementedEvent = EventManager.RegisterRoutedEvent(
            "IsIncremented",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SpinnerInputField));


        public event RoutedEventHandler IsDecremented
        {
            add { AddHandler(IsDecrementedEvent, value); }
            remove { RemoveHandler(IsDecrementedEvent, value); }
        }

        public static readonly RoutedEvent IsDecrementedEvent = EventManager.RegisterRoutedEvent(
            "IsDecremented",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SpinnerInputField));

        public event RoutedEventHandler NumberChanged
        {
            add { AddHandler(NumberChangedEvent, value); }
            remove { RemoveHandler(NumberChangedEvent, value); }
        }

        public static readonly RoutedEvent NumberChangedEvent = EventManager.RegisterRoutedEvent(
            "NumberChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SpinnerInputField));

    }
}
