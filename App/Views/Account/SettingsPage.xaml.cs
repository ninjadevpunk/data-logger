using Data_Logger_1._3.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3.Views.Account
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly Storyboard _open;
        private readonly Storyboard _passwordOpen;
        private readonly Storyboard _close;
        private readonly Storyboard _passwordClose;
        public bool ResetLinkSectionOpen { get; set; } = false;
        public bool ChangePasswordSectionOpen { get; set; } = false;

        public SettingsPage()
        {
            InitializeComponent();

            _open = CreateHeightAnimation(0, 100, 200);
            _passwordOpen = CreateHeightAnimation(0, 260, 200);
            _close = CreateHeightAnimation(100, 0, 200);
            _close.Completed += ResetLinkClose_Completed;
            _passwordClose = CreateHeightAnimation(260, 0, 200);
            _passwordClose.Completed += PasswordClose_Completed;

            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SettingsViewModel.ResetStage))
                return;

            var vm = (SettingsViewModel)sender!;

            switch (vm.ResetStage)
            {
                case SettingsViewModel.PasswordResetStage.RequestReset:
                    break;

                case SettingsViewModel.PasswordResetStage.EnterVerificationCode:
                    {
                        if (ChangePasswordSectionOpen)
                            _passwordClose.Begin(this.stackPanel_NEW_PASSWORD_SECTION);

                        if (!ResetLinkSectionOpen)
                        {
                            _open.Begin(this.stackPanel_PASSWORD_RESET_SECTION);
                            ResetLinkSectionOpen = true;
                        }

                        break;
                    }
                case SettingsViewModel.PasswordResetStage.ChangePassword:
                    {
                        if (ResetLinkSectionOpen)
                            _close.Begin(this.stackPanel_PASSWORD_RESET_SECTION);

                        break;
                    }
                case SettingsViewModel.PasswordResetStage.PasswordChanged:
                    {
                        if (ChangePasswordSectionOpen)
                            _passwordClose.Begin(this.stackPanel_NEW_PASSWORD_SECTION);

                        break;
                    }
            }
        }


        private void PasswordClose_Completed(object? sender, EventArgs e)
        {
            ChangePasswordSectionOpen = false;
        }

        private void ResetLinkClose_Completed(object? sender, EventArgs e)
        {
            ResetLinkSectionOpen = false;

            if (!ChangePasswordSectionOpen)
            {
                _passwordOpen.Begin(this.stackPanel_NEW_PASSWORD_SECTION);
                ChangePasswordSectionOpen = true;
            }
        }


        #region Animations 


        private static Storyboard CreateHeightAnimation(double from, double to, int durationMs)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(durationMs)
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            Storyboard.SetTargetProperty(animation, new PropertyPath(StackPanel.HeightProperty));

            return storyboard;
        }



        #endregion
    }
}
