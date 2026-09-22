using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.LoggerCommands
{
    public class SaveCommand : AsyncCommandBase
    {
        private readonly LoggerCreateViewModel _loggerCreateViewModel;
        private readonly IDataService _dataService;
        private ApplicationClass QtCreator;
        private ApplicationClass AndroidStudio;


        public SaveCommand(LoggerCreateViewModel loggerCreateViewModel, IDataService dataService)
        {
            try
            {
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception found: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");
            }
        }

        protected async override Task ExecuteAsync(object parameter)
        {
            try
            {
                QtCreator = await _dataService.FindApplicationByID(1);
                AndroidStudio = await _dataService.FindApplicationByID(2);

                var dialog = new Microsoft.Win32.SaveFileDialog();
                ProjectClass project = new(_loggerCreateViewModel.ProjectName);
                OutputClass output = new(_loggerCreateViewModel.Output);
                TypeClass type = new(_loggerCreateViewModel.Type);
                List<PostIt> postItsList = new();
                PostIt postIt;
                SubjectClass subject;

                foreach (var item in _loggerCreateViewModel.PostIts)
                {
                    postIt = new();

                    postIt.postItID = 0;

                    subject = new(item.Subject);

                    postIt.Subject = subject;

                    postIt.Error = item.Error;
                    postIt.ERCaptureTime = item.DateFound;
                    postIt.Solution = item.Solution;
                    postIt.SOCaptureTime = item.DateSolved;
                    postIt.Suggestion = item.Suggestion;
                    postIt.Comment = item.Comment;

                    postItsList.Add(postIt);
                }

                switch (_loggerCreateViewModel.Context)
                {
                    case CacheContext.Qt:
                        {
                            dialog.DefaultExt = ".log";
                            dialog.Filter = "Qt LOG (.log)|*.log|JSON (.json)|*.json";

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var codingCreator = (codeCreateViewModel)_loggerCreateViewModel;
                                _dataService.SaveLOG(new CodingLOG(
                                        0,
                                        _dataService.GetUser(),
                                        project,
                                        QtCreator,
                                        codingCreator.StartDate,
                                        codingCreator.EndDate,
                                        output,
                                        type,
                                        postItsList,
                                        codingCreator.BugsFound,
                                        codingCreator.ApplicationOpened
                                        ),
                                        dialog.FileName);
                            }
                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            break;
                        }
                    case CacheContext.Coding:
                        {
                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            break;
                        }
                    case CacheContext.Film:
                        {
                            break;
                        }
                    case CacheContext.NOTES:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found: {ex.Message}");
            }
        }
    }
}
