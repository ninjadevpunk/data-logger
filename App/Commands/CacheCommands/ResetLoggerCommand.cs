using Core.Models;
using Core.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.CacheCommands
{
    internal class ResetLoggerCommand : CommandBase
    {
        private readonly LoggerViewModel _loggerViewModel;
        private readonly LOG.CATEGORY _category;

        public ResetLoggerCommand(LoggerViewModel loggerViewModel, LOG.CATEGORY category = LOG.CATEGORY.CODING)
        {
            _loggerViewModel = loggerViewModel;
            _category = category;
        }


        public override void Execute(object parameter)
        {
            try
            {
                var isQt = _loggerViewModel.ApplicationName.Equals(Constants.Qt);
                var isVisualStudio = _loggerViewModel.ApplicationName.Equals(Constants.VisualStudio);

                _loggerViewModel.ProjectName = string.Empty;
                _loggerViewModel.ApplicationName = string.Empty;
                _loggerViewModel.StartDate = DateTime.Now;
                _loggerViewModel.StartHours = DateTime.Now.Hour;
                _loggerViewModel.StartMinutes = DateTime.Now.Minute;
                _loggerViewModel.StartSeconds = DateTime.Now.Second;
                _loggerViewModel.StartMilliseconds = DateTime.Now.Millisecond;

                _loggerViewModel.EndDate = DateTime.Now;
                _loggerViewModel.EndHours = DateTime.Now.Hour;
                _loggerViewModel.EndMinutes = DateTime.Now.Minute;
                _loggerViewModel.EndSeconds = DateTime.Now.Second;
                _loggerViewModel.EndMilliseconds = DateTime.Now.Millisecond;
                _loggerViewModel.PostIts.Clear();

                switch (_category)
                {
                    case LOG.CATEGORY.CODING:
                        {
                            var codingViewModel = (codeCreateViewModel)_loggerViewModel;
                            codingViewModel.ProjectName = string.Empty;

                            if (isQt)
                                codingViewModel.ApplicationName = Constants.Qt;
                            else if (isVisualStudio)
                                codingViewModel.ApplicationName = Constants.VisualStudio;

                            codingViewModel.Output = "Widgets Application";
                            codingViewModel.Type = "Build";
                            codingViewModel.BugsFound = 0;
                            codingViewModel.ApplicationOpened = false;

                            if (_loggerViewModel is AScodeCreateViewModel AScodingViewModel)
                            {
                                AScodingViewModel.ApplicationName = Constants.Android;
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
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near ResetLoggerCommand: {e.Message}");
            }
        }
    }
}
