using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for Updater.xaml
    /// </summary>
    public partial class VelopackUpdaterWindow : Window
    {
        private readonly Storyboard _startUpdating;
        private Border? _borderWindow;
        private readonly bool _startUpdatingImmediately;

        public VelopackUpdaterWindow()
        {
            InitializeComponent();

            _startUpdating = CreateUpdatingAnimation();
        }

        public VelopackUpdaterWindow(bool startUpdatingImmediately = false)
        {
            InitializeComponent();

            _startUpdatingImmediately = startUpdatingImmediately;
            _startUpdating = CreateUpdatingAnimation();

            if (_startUpdatingImmediately)
                Loaded += on_UPDATE_NOW_Clicked;
        }


        private void StartUpdatingAnimation()
        {
            _startUpdating.Begin(this);
        }

        private static Storyboard CreateUpdatingAnimation()
        {
            var storyboard = new Storyboard();
            var duration = TimeSpan.FromMilliseconds(250);

            storyboard.Children.Add(CreateOpacityAnimation(1, 0, duration, "grid_UPDATER_HEADER"));
            storyboard.Children.Add(CreateOpacityAnimation(1, 0, duration, "grid_UPDATE_BUTTONS"));
            storyboard.Children.Add(CreateOpacityAnimation(1, 0, duration, "grid_DONT_SHOW_AGAIN"));

            storyboard.Children.Add(CreateOpacityAnimation(0, 1, duration, "textBlock_UPDATING"));
            storyboard.Children.Add(CreateOpacityAnimation(0, 1, duration, "progressBar_UPDATE"));

            return storyboard;
        }




        #region Animations



        private static DoubleAnimation CreateOpacityAnimation(double from, double to, TimeSpan duration, string targetName)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = duration
            };

            Storyboard.SetTargetName(animation, targetName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));

            return animation;
        }



        #endregion








        private void on_UPDATE_NOW_Clicked(object sender, RoutedEventArgs e)
        {
            this.textBlock_MESSAGE.Text = "The application will restart automatically once complete.";
            this.grid_BUTTONS_AND_CHECKBOX.Margin = new Thickness(this.grid_BUTTONS_AND_CHECKBOX.Margin.Left, 12,
                this.grid_BUTTONS_AND_CHECKBOX.Margin.Right, this.grid_BUTTONS_AND_CHECKBOX.Margin.Bottom);
            StartUpdatingAnimation();
        }

        private void on_VelopackUpdaterWindow_Pressed(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
