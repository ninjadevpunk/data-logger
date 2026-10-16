namespace Core.Utilities
{
    public static class Constants
    {
        public const string Qt = "Qt Creator";
        public const string Android = "Android Studio Quail 4 — 2026.1.4";
        public const string VisualStudio = "Visual Studio Community 2022";
        public const string Avatar = "/Assets/login/user.png";
        public const string ExceptionPrefix = "Exception occurred in";

#if DEBUG
        public static string SettingsBasePath = Path.Combine(AppContext.BaseDirectory, "Data Logger");
#else
        public static string SettingsBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger");
#endif
    }
}
