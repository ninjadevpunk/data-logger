using Core.Models.App_Models;

namespace Core.Models
{
    public enum Theme
    {
        Default,
        Light,
        Grey
    }

    public class Settings
    {

        // AlphaBeta builds are Grey themed only
        public Theme AppTheme { get; set; } = Theme.Default;

        public UserSettings User { get; set; } = new UserSettings();

        public bool ShowUpdatePopup { get; set; } = true;

        public void SelectedTheme(string theme)
        {
            switch (theme)
            {
                case "Light":
                    {
                        AppTheme = Theme.Light;
                        break;
                    }
                case "Grey":
                    {
                        AppTheme = Theme.Grey;
                        break;
                    }
                default:
                    {
                        AppTheme = Theme.Default;
                        return;
                    }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Settings other &&
                   AppTheme == other.AppTheme &&
                   Equals(User, other.User) &&
                   ShowUpdatePopup == other.ShowUpdatePopup;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AppTheme, User, ShowUpdatePopup);
        }

        public static bool operator ==(Settings? left, Settings? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Settings? left, Settings? right)
        {
            return !Equals(left, right);
        }
    }
}
