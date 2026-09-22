using Data_Logger_1._3.ViewModels.Reporter;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Data_Logger_1._3.Components
{

    /// <summary>
    /// A custom search bar for the reporter.
    /// </summary>
    public class SearchBarControl : Control
    {
        static SearchBarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBarControl), new FrameworkPropertyMetadata(typeof(SearchBarControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Loaded += (sender, e) =>
            {
                UserComboInput = "";

                

                var textBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
                if (textBox != null)
                {
                    textBox.TextChanged -= on_EditableTextBox_changed;
                    textBox.LostFocus -= on_EditableTextBox_lostfocus;

                    textBox.TextChanged += on_EditableTextBox_changed;
                    textBox.LostFocus += on_EditableTextBox_lostfocus;
                }

                var mainWindow = Application.Current.MainWindow;

                if (mainWindow != null)
                {
                    var popup = GetTemplateChild("PART_Popup") as Popup;
                    Application.Current.MainWindow.PreviewMouseDown += (s, e) =>
                    {
                        if (popup != null && textBox != null)
                        {
                            // Only if click is outside popup and textbox
                            if (!popup.IsMouseOver && !textBox.IsMouseOver)
                            {
                                this.IsDropDownOpen = false;
                            }
                        }
                    };
                }
            };




        }














        #region Dependency Properties







        // Placeholder text
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(SearchBarControl), new("Search"));



        public string DynamicPlaceholderText
        {
            get { return (string)GetValue(DynamicPlaceholderTextProperty); }
            set { SetValue(DynamicPlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty DynamicPlaceholderTextProperty =
            DependencyProperty.Register("DynamicPlaceholderText", typeof(string), typeof(SearchBarControl));




        // User's input
        public string UserComboInput
        {
            get { return (string)GetValue(UserComboInputProperty); }
            set { SetValue(UserComboInputProperty, value); }
        }

        public static readonly DependencyProperty UserComboInputProperty =
            DependencyProperty.Register("UserComboInput", typeof(string), typeof(SearchBarControl), new(""));


        // For enabling and disabling editing the control. Will allow it to be a normal combo box if false.
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(SearchBarControl), new PropertyMetadata(true));



        // Style for the TextBox in the ComboBox
        public Style UserTextBoxStyle
        {
            get { return (Style)GetValue(UserTextBoxStyleProperty); }
            set { SetValue(UserTextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBoxStyleProperty =
            DependencyProperty.Register("UserTextBoxStyle", typeof(Style), typeof(SearchBarControl));


        // TextBlock Style
        public Style UserTextBlockStyle
        {
            get { return (Style)GetValue(UserTextBlockStyleProperty); }
            set { SetValue(UserTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBlockStyleProperty =
            DependencyProperty.Register("UserTextBlockStyle", typeof(Style), typeof(SearchBarControl));


        public bool HasNoResults
        {
            get { return (bool)GetValue(HasNoResultsProperty); }
            set{SetValue(HasNoResultsProperty, value);}
        }

        public static readonly DependencyProperty HasNoResultsProperty =
            DependencyProperty.Register(nameof(HasNoResults), typeof(bool), typeof(SearchBarControl), new(true));

        // List of SearchResults
        public ObservableCollection<SearchResultViewModel> SearchResults
        {
            get { return (ObservableCollection<SearchResultViewModel>)GetValue(SearchResultsProperty); }
            set { SetValue(SearchResultsProperty, value); }
        }

        public static readonly DependencyProperty SearchResultsProperty =
            DependencyProperty.Register("SearchResults", typeof(ObservableCollection<SearchResultViewModel>), typeof(SearchBarControl),
                new PropertyMetadata(new ObservableCollection<SearchResultViewModel>(), on_ResultsList_Changed));



        // SearchCommand Bar Opens and Closes Popup
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(SearchBarControl), new PropertyMetadata(false, on_Search_Clicked));



        // Enter Button Must Execute SearchCommand

        public ICommand EnterInternalCommand
        {
            get { return (ICommand)GetValue(EnterInternalCommandProperty); }
            set { SetValue(EnterInternalCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterInternalCommandProperty =
            DependencyProperty.Register("EnterInternalCommand", typeof(ICommand), typeof(SearchBarControl));





        #endregion







        #region Member Functions



        private void on_EditableTextBox_lostfocus(object sender, RoutedEventArgs e)
        {
            UpdatePlaceholderText();
            IsDropDownOpen = false;
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
            IsDropDownOpen = false;

            var ev = new RoutedEventArgs(OnTextChangedEvent);
            RaiseEvent(ev);
        }

        private static void on_Search_Clicked(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SearchBarControl)d;

            bool isOpen = (bool)e.NewValue;

            // Handle open
            if (isOpen)
            {
                ctrl.OnPopupOpened();
            }
            else
            {
                ctrl.OnPopupClosed();
            }
        }

        private void OnPopupOpened()
        {
            // Update empty state immediately
            UpdateHasNoResults();
        }

        private void OnPopupClosed()
        {
            HasNoResults = true;
        }


        private static void on_ResultsList_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SearchBarControl)d;

            // Unsubscribe from old collection
            if (e.OldValue is ObservableCollection<SearchResultViewModel> oldCollection)
                oldCollection.CollectionChanged -= ctrl.SearchResults_CollectionChanged;

            // Subscribe to new collection
            if (e.NewValue is ObservableCollection<SearchResultViewModel> newCollection)
                newCollection.CollectionChanged += ctrl.SearchResults_CollectionChanged;

            // Update immediately
            ctrl.UpdateHasNoResults();
        }

        private void SearchResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateHasNoResults();
        }

        private void UpdateHasNoResults()
        {
            HasNoResults = SearchResults == null || SearchResults.Count == 0;
        }





        #endregion











        #region Routed Events


        public event RoutedEventHandler OnTextChanged
        {
            add { AddHandler(OnTextChangedEvent, value); }
            remove { RemoveHandler(OnTextChangedEvent, value); }
        }

        public static readonly RoutedEvent OnTextChangedEvent = EventManager.RegisterRoutedEvent(
            "OnTextChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SearchBarControl));






        #endregion



















    }
}
