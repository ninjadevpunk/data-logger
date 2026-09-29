using Core.DTOs.Cache;
using Core.Enums;
using Core.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.DTOs;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class EditCommand : AsyncCommandBase
    {
        private readonly LogCacheDTO _logCacheDTO;
        private readonly NavigationService _navigationService;
        private readonly LogContext _cacheContext;

        public EditCommand(LogContext logContext, NavigationService navigationService, LogCacheDTO logCacheDTO)
        {
            _logCacheDTO = logCacheDTO ?? throw new ArgumentNullException(nameof(logCacheDTO));
            _cacheContext = logContext;
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                switch (_cacheContext)
                {
                    case LogContext.Qt:
                        {
                            

                            break;
                        }
                    case LogContext.AndroidStudio:
                        {

                            break;
                        }
                    case LogContext.Coding:
                        {
                            var codeLogViewModel = parameter as CodeLOGViewModel;

                            if (LogIsNull(codeLogViewModel))
                                throw new InvalidOperationException("Log is null!");

                            await _navigationService.NavigateToLoggerEditor(codeLogViewModel!, _logCacheDTO);

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in EditCommand.ExecuteAsync: {ex.Message}");
            }
        }

        private bool LogIsNull(LOGViewModel? log)
        {
            return log == null;
        }
    }
}
