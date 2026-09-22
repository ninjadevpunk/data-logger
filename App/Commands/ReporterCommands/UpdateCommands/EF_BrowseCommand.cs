using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_BrowseCommand : CommandBase
    {
        private readonly CacheContext _cacheContext;
        private readonly ReporterUpdaterViewModel _reporterUpdaterViewModel;

        public EF_BrowseCommand(CacheContext cacheContext, ReporterUpdaterViewModel reporterUpdaterViewModel)
        {
            try
            {
                _cacheContext = cacheContext;
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near EF_BrowseCommand(cacheContext,loggerCreateViewModel) constructor: {ex.Message}");
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
                            graphicsUpdateViewModel graphicsCVM = (graphicsUpdateViewModel)_reporterUpdaterViewModel;

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
                            filmUpdateViewModel filmCVM = (filmUpdateViewModel)_reporterUpdaterViewModel;

                            dialog.DefaultExt = ".mp4";
                            dialog.Filter = @"Motion Picture Experts Group Layer 4 (.mp4)|*.mp4|Matroska (.mkv)|*.mkv|
                                            Windows Media Video (.wmv)|*.wmv|Transfer Stream (.ts)|*.ts|WebM (.webm)|*.webm;";

                            bool? result = dialog.ShowDialog();

                            if (result == true)
                                filmCVM.Source = dialog.FileName;

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            flexiUpdateViewModel flexiCVM = (flexiUpdateViewModel)_reporterUpdaterViewModel;

                            bool? result = dialog.ShowDialog();

                            if (result == true)
                                flexiCVM.Source = dialog.FileName;

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near EF_BrowseCommand.Execute: {ex.Message}");
            }
        }
    }
}
