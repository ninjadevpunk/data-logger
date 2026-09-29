using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.Services
{
    /// <summary>
    /// Class for BitmapImage management and related tasks.
    /// </summary>
    internal class BitmapService
    {
        /// <summary>
        /// Helper method to resize an image to a maximum dimension of 128 pixels while maintaining the aspect ratio.
        /// </summary>
        /// <param name="path">The file path to the image</param>
        /// <returns>Returns the resized image</returns>
        private static TransformedBitmap Resizer(string path)
        {
            var bitmap = new BitmapImage(new Uri(path));

            int size = 128;

            double scale = Math.Min(
                (double)size / bitmap.PixelWidth,
                (double)size / bitmap.PixelHeight
            );

            return new TransformedBitmap(bitmap, new ScaleTransform(scale, scale));
        }

        // TODO: Delete
        /// <summary>
        /// Saves a temporary profile picture to the permanent profile picture directory.
        /// </summary>
        /// <param name="tempPath">The temporary profile picture path.</param>
        /// <returns>Returns the permanent file path, or an empty string if the save failed.</returns>
        public static string SaveProfilePicture(string tempPath)
        {
            try
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger", "Profile Pics");

                Directory.CreateDirectory(folder);

                string newPath = Path.Combine(folder, $"{Guid.NewGuid()}.png");

                if (tempPath.Contains("/Assets"))
                    return string.Empty;

                File.Copy(tempPath, newPath, overwrite: false);

                return newPath;
            }
            catch (Exception ex)
            {
                //App.LogExceptionAsync(ex);
                Debug.WriteLine(ex, "Exception occurred in SaveProfilePicture");
                return string.Empty;
            }
        }

        /// <summary>
        /// Saves a resized image to a temporary folder in the user's temp directory. This is used for profile pictures before they are permanently saved.
        /// </summary>
        /// <param name="path">The file path to the real image</param>
        /// <returns>Returns the path to the saved image</returns>
        public static string TempSaveResizedImage(string path)
        {
            try
            {
                string folder = Path.Combine(Path.GetTempPath(), "Data Logger", "Temp Profile Pics");
                Directory.CreateDirectory(folder);
                string newPath = Path.Combine(folder, Guid.NewGuid() + ".png");

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(Resizer(path)));
                    encoder.Save(stream);
                }

                return newPath;
            }
            catch (Exception ex)
            {
                //App.LogExceptionAsync(ex);
                Debug.WriteLine(ex, "Exception occurred in TempSaveResizedImage");
                return string.Empty;
            }
        }

        // TODO: DELETE
        /// <summary>
        /// Deletes profile pics saved in temp folder created by Browse button logic
        /// </summary>
        public static void DeleteTempProfilePics()
        {
            try
            {
                // Save to User's Temp folder
                string folder = Path.Combine(Path.GetTempPath(), "Data Logger", "Temp Profile Pics");

                if (!Directory.Exists(folder))
                    return;

                // Delete all files.
                foreach (var file in Directory.GetFiles(folder))
                {
                    File.Delete(file);
                }

                // Delete any subdirectories and their contents.
                foreach (var directory in Directory.GetDirectories(folder))
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
                //App.LogExceptionAsync(ex);
                Debug.WriteLine(ex, "Exception occurred in DeleteTempProfilePics");
            }
        }

        public static BitmapImage? LoadImage(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        public static bool IsTemporaryProfilePic(string path)
        {
            try
            {
                string tempFolder = Path.Combine(Path.GetTempPath(), "Data Logger", "Temp Profile Pics");
                return path.StartsWith(tempFolder, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                //App.LogExceptionAsync(ex);
                Debug.WriteLine(ex, "Exception occurred in LoadImage");
            }
            return false;
        }
    }
}
