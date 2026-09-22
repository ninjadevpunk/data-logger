using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_ResetLoggerCommand : CommandBase
    {
        private readonly ReporterUpdaterViewModel _viewModel;
        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Meerkat 2024.3.1";
        private const string VisualStudio = "Visual Studio Community 2022";

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public EF_ResetLoggerCommand(ReporterUpdaterViewModel viewModel, LOG.CATEGORY category)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                Category = category;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near EF_ResetLoggerCommand(viewModel, category) constructor: {ex.Message}");
            }
        }

        public EF_ResetLoggerCommand()
        {

        }

        public override void Execute(object parameter)
        {
            try
            {
                var isQt = _viewModel.ApplicationName.Equals(Qt);
                var isAndroidStudio = _viewModel.ApplicationName.Equals(Android);
                var isVisualStudio = _viewModel.ApplicationName.Equals(VisualStudio);

                _viewModel.ProjectName = string.Empty;
                _viewModel.ApplicationName = string.Empty;
                _viewModel.StartDate = DateTime.Now;
                _viewModel.StartHours = DateTime.Now.Hour;
                _viewModel.StartMinutes = DateTime.Now.Minute;
                _viewModel.StartSeconds = DateTime.Now.Second;
                _viewModel.StartMilliseconds = DateTime.Now.Millisecond;

                _viewModel.EndDate = DateTime.Now;
                _viewModel.EndHours = DateTime.Now.Hour;
                _viewModel.EndMinutes = DateTime.Now.Minute;
                _viewModel.EndSeconds = DateTime.Now.Second;
                _viewModel.EndMilliseconds = DateTime.Now.Millisecond;
                _viewModel.PostIts.Clear();

                switch (Category)
                {
                    case LOG.CATEGORY.CODING:
                        {
                            var codingViewModel = (codeUpdateViewModel)_viewModel;

                            if (isQt)
                                codingViewModel.ApplicationName = Qt;
                            else if (isVisualStudio)
                                codingViewModel.ApplicationName = VisualStudio;

                            codingViewModel.Output = "Widgets Application";
                            codingViewModel.Type = "Build";
                            codingViewModel.BugsFound = 0;
                            codingViewModel.ApplicationOpened = false;

                            if (isAndroidStudio)
                            {
                                codingViewModel.ApplicationName = Android;
                                var AScodingViewModel = (AScodeUpdateViewModel)codingViewModel;
                                AScodingViewModel.IsSimple = false;

                                AScodingViewModel.SyncHours = 0;
                                AScodingViewModel.SyncMinutes = 0;
                                AScodingViewModel.SyncSeconds = 0;
                                AScodingViewModel.SyncMilliseconds = 0;

                                AScodingViewModel.IsConsidered = false;
                                AScodingViewModel.GradleDaemonHours = 0;
                                AScodingViewModel.GradleDaemonMinutes = 0;
                                AScodingViewModel.GradleDaemonSeconds = 0;
                                AScodingViewModel.GradleDaemonMilliseconds = 0;

                                AScodingViewModel.RunBuildHours = 0;
                                AScodingViewModel.RunBuildMinutes = 0;
                                AScodingViewModel.RunBuildSeconds = 0;
                                AScodingViewModel.RunBuildMilliseconds = 0;

                                AScodingViewModel.LoadBuildHours = 0;
                                AScodingViewModel.LoadBuildMinutes = 0;
                                AScodingViewModel.LoadBuildSeconds = 0;
                                AScodingViewModel.LoadBuildMilliseconds = 0;

                                AScodingViewModel.ConfigureBuildHours = 0;
                                AScodingViewModel.ConfigureBuildMinutes = 0;
                                AScodingViewModel.ConfigureBuildSeconds = 0;
                                AScodingViewModel.ConfigureBuildMilliseconds = 0;

                                AScodingViewModel.AllProjectsHours = 0;
                                AScodingViewModel.AllProjectsMinutes = 0;
                                AScodingViewModel.AllProjectsSeconds = 0;
                                AScodingViewModel.AllProjectsMilliseconds = 0;

                            }
                            break;
                        }
                    case LOG.CATEGORY.GRAPHICS:
                        {
                            var graphicsViewModel = (graphicsUpdateViewModel)_viewModel;
                            graphicsViewModel.Output = "Portable Network Graphics (*.PNG)";
                            graphicsViewModel.Type = "NONE";
                            graphicsViewModel.Medium = "Pencil";
                            graphicsViewModel.Format = "Paper";
                            graphicsViewModel.Brush = string.Empty;
                            graphicsViewModel.Height = string.Empty;
                            graphicsViewModel.Width = string.Empty;
                            graphicsViewModel.MeasuringUnit = "cm";
                            graphicsViewModel.Size = "A4 (29,7 cm x 21 cm)";
                            graphicsViewModel.DPI = string.Empty;
                            graphicsViewModel.ColourDepth = string.Empty;
                            graphicsViewModel.IsCompleted = false;
                            graphicsViewModel.Source = string.Empty;

                            break;
                        }
                    case LOG.CATEGORY.FILM:
                        {
                            var filmViewModel = (filmUpdateViewModel)_viewModel;
                            filmViewModel.Height = string.Empty;
                            filmViewModel.Width = string.Empty;
                            filmViewModel.Resolution = string.Empty;
                            filmViewModel.Length = string.Empty;
                            filmViewModel.IsCompleted = false;
                            filmViewModel.Source = string.Empty;

                            break;
                        }
                    case LOG.CATEGORY.NOTES:
                        {
                            var flexiViewModel = (flexiUpdateViewModel)_viewModel;
                            flexiViewModel.FlexibleLogCategory = string.Empty;
                            flexiViewModel.Medium = "Song";
                            flexiViewModel.Format = "CD";
                            flexiViewModel.Bitrate = string.Empty;
                            flexiViewModel.Duration = string.Empty;
                            flexiViewModel.IsCompleted = false;
                            flexiViewModel.Source = string.Empty;

                            break;
                        }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near EF_ResetLoggerCommand: {e.Message}");
                // TODO
            }
        }

    }
}
