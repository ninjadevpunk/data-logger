using System.Windows;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3
{
    public class DLSWindow : Window
    {
        protected string AppVersion { get; }

        public DLSWindow()
        {
            Application.Current.MainWindow = this;

            string version = App.Configuration?["App:Version"] ?? string.Empty;
            AppVersion = FormatVersion(version);

#if DEBUG
            Uri iconUri = new Uri("pack://application:,,,/DevIcon.ico");
#else
            string iconFile = version switch
            {
                var v when v.Contains("-alpha.", StringComparison.OrdinalIgnoreCase)
                    => "AlphaBetaIcon.ico",

                var v when v.Contains("-beta.", StringComparison.OrdinalIgnoreCase)
                    => "AlphaBetaIcon.ico",

                _ => "ReleaseIcon.ico"
            };

            Uri iconUri = new Uri($"pack://application:,,,/{iconFile}");
#endif

            this.Icon = BitmapFrame.Create(iconUri);
        }



        private static string FormatVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return string.Empty;

            string[] parts = version.Split('-', 2);

            // 1.3.0
            if (parts.Length == 1)
                return parts[0];

            // alpha.1 / beta.1 / rc.1
            string[] prereleaseParts = parts[1].Split('.', 2);

            if (prereleaseParts.Length != 2)
                return string.Empty;

            string channel = prereleaseParts[0].ToLowerInvariant();
            string number = prereleaseParts[1];

            return channel switch
            {
                "alpha" => $"alpha {number}",
                "beta" => $"beta {number}",
                "rc" => $"RC {number}",
                _ => string.Empty
            };
        }
    }
}
