using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.LoggerCommands
{
    public class BrowseCommand : CommandBase
    {
        private readonly CacheContext _cacheContext;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;

        public BrowseCommand(CacheContext cacheContext, LoggerCreateViewModel loggerCreateViewModel)
        {
            try
            {
                _cacheContext = cacheContext;
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near BrowseCommand(cacheContext,loggerCreateViewModel) constructor: {ex.Message}");
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();

                switch (_cacheContext)
                {
                    case CacheContext.Graphics:
                        {
                            graphicCreateViewModel graphicsCVM = (graphicCreateViewModel)_loggerCreateViewModel;

                            dialog.DefaultExt = ".png"; // Default file extension
                            dialog.Filter = @"Portable Network Graphics (.png)|*.png|JPEG Images (.jpg)|*.jpg|
                                            Scalable Vector Graphics (.svg)|*.svg|WebP (.webp)|*.webp|
                                            Graphics Interchange Format (.gif)|*.gif|Tag Image File Format (.tiff)|*.tiff;"; // Filter files by extension

                            // Show open file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process open file dialog box results
                            if (result == true)
                                graphicsCVM.Source = dialog.FileName;

                            break;
                        }
                    case CacheContext.Film:
                        {
                            filmCreateViewModel filmCVM = (filmCreateViewModel)_loggerCreateViewModel;

                            dialog.DefaultExt = ".mp4";
                            dialog.Filter = @"Motion Picture Experts Group Layer 4 (.mp4)|*.mp4|Matroska (.mkv)|*.mkv|
                                            Windows Media Video (.wmv)|*.wmv|Transfer Stream (.ts)|*.ts|WebM (.webm)|*.webm;";

                            bool? result = dialog.ShowDialog();

                            if(result == true)
                                filmCVM.Source = dialog.FileName;

                            break;
                        }
                        case CacheContext.Flexi:
                        {
                            flexiCreateViewModel flexiCVM = (flexiCreateViewModel)_loggerCreateViewModel;

                            bool? result = dialog.ShowDialog();

                            if (result == true)
                                flexiCVM.Source = dialog.FileName;

                                break;
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near BrowseCommand.Execute: {ex.Message}");
            }
        }
    }
}
