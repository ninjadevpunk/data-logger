using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for PasswordInputField.xaml
    /// </summary>
    public partial class PasswordInputField : UserControl
    {
        public bool Status { get; set; } = true;
        public string Temp { get; set; } = "";

        public bool IsBeingEdited { get; set; } = false;

        public PasswordInputField()
        {
            InitializeComponent();
        }


        #region Dependency Properties


        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PasswordInputField));


        public string UserInput
        {
            get { return (string)GetValue(UserInputProperty); }
            set { SetValue(UserInputProperty, value); }
        }

        public static readonly DependencyProperty UserInputProperty =
            DependencyProperty.Register("UserInput", typeof(string), typeof(PasswordInputField),

                new PropertyMetadata(string.Empty, ViewModelPasswordChanged));



        public string PasswordHash
        {
            get { return (string)GetValue(PasswordHashProperty); }
            set { SetValue(PasswordHashProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasswordHash.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordHashProperty =
            DependencyProperty.Register("PasswordHash", typeof(string), typeof(PasswordInputField));





        // Text Alignment
        public string Aligned
        {
            get { return (string)GetValue(AlignedProperty); }
            set { SetValue(AlignedProperty, value); }
        }

        public static readonly DependencyProperty AlignedProperty =
            DependencyProperty.Register("Aligned", typeof(string), typeof(PasswordInputField));






        #endregion


        #region Event Handlers

        private void on_Password_changed(object sender, RoutedEventArgs e)
        {
            if (Status)
            {
                Temp = this.text_Placeholder_text.Text;
                this.text_Placeholder_text.Text = "";
                Status = false;

            }

            Password_changed();

            var ev = new RoutedEventArgs(PasswordChangedEvent);
            RaiseEvent(ev);

        }

        private static void ViewModelPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is PasswordInputField passwordField)
            {
                passwordField.ViewModelPasswordChanged();
            }

        }

        public void Password_changed()
        {
            IsBeingEdited = true;
            UserInput = this.control_inputText_Password.Password;
            IsBeingEdited = false;
        }

        public void ViewModelPasswordChanged()
        {
            if (!IsBeingEdited)
                this.control_inputText_Password.Password = UserInput;
        }



        private void on_Password_lostFocus(object sender, RoutedEventArgs e)
        {
            showPlaceholderText();
        }


        public void showPlaceholderText()
        {
            if (this.control_inputText_Password.Password == null)
            {
                return;
            }
            else if (this.control_inputText_Password.Password == "" && this.text_Placeholder_text.Text == "")
            {
                this.text_Placeholder_text.Text = Temp;
                Status = true;
            }
        }



        #endregion







        #region Routed Events


        public event RoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }

        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
            "PasswordChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PasswordInputField));








        #endregion

    }
}
