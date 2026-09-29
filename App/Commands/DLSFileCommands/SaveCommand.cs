using Core.DTOs.Cache;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.DLSFileCommands
{
    /// <summary>
    /// Saving a .dls file in the logger UI
    /// </summary>
    internal class SaveCommand : AsyncCommandBase
    {
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cachemaster;
        private ApplicationClass? QtCreator;
        private ApplicationClass? AndroidStudio;


        public SaveCommand(LoggerViewModel loggerCreateViewModel, IAuthService authService, IDataService dataService, ICacheService cacheService)
        {
            _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _cachemaster = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        protected async override Task ExecuteAsync(object parameter)
        {
            try
            {

                var user = _authService.GetUser() ?? throw new InvalidOperationException("User is null");

                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.DefaultExt = ".dls";


                switch (_loggerCreateViewModel.Context)
                {
                    case LogContext.Qt:
                        {
                            dialog.Filter = "Qt LOG file (.dls)|*.dls|JSON (.json)|*.json";

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var qtCreator = (QTcodeCreateViewModel)_loggerCreateViewModel;
                                _cachemaster.SaveLOG(new QtCacheDTO
                                {
                                    UserId = user.UserId,
                                    ProjectName = qtCreator.ProjectName,
                                    Start = qtCreator.StartDate,
                                    End = qtCreator.EndDate,
                                    OutputName = qtCreator.Output,
                                    TypeName = qtCreator.Type,
                                    PostIts = qtCreator.PostIts.Select(p => new PostItCacheDTO
                                    {
                                        Subject = p.Subject,
                                        Error = p.Error,
                                        Solution = p.Solution,
                                        Suggestion = p.Suggestion,
                                        Comment = p.Comment,
                                    }).ToList(),
                                    Bugs = qtCreator.BugsFound,
                                    Success = qtCreator.ApplicationOpened
                                }, dialog.FileName);
                            }
                            break;
                        }
                    case LogContext.AndroidStudio:
                        {
                            break;
                        }
                    case LogContext.Coding:
                        {
                            dialog.Filter = "LOG file (.dls)|*.dls|JSON (.json)|*.json";

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var codingCreator = (codeCreateViewModel)_loggerCreateViewModel;
                                _cachemaster.SaveLOG(new CodingCacheDTO
                                {
                                    UserId = user.UserId,
                                    ProjectName = codingCreator.ProjectName,
                                    ApplicationName = codingCreator.ApplicationName,
                                    Start = codingCreator.StartDate,
                                    End = codingCreator.EndDate,
                                    OutputName= codingCreator.Output,
                                    TypeName = codingCreator.Type,
                                    PostIts = codingCreator.PostIts.Select(p => new PostItCacheDTO
                                    {
                                        Subject = p.Subject,
                                        Error = p.Error,
                                        Solution = p.Solution,
                                        Suggestion = p.Suggestion,
                                        Comment = p.Comment
                                    }).ToList(),
                                    Bugs = codingCreator.BugsFound,
                                    Success = codingCreator.ApplicationOpened
                                }, dialog.FileName);
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception found in SaveCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
