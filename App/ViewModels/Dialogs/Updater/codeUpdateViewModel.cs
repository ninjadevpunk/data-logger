using Core.DTOs.Data;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Updater
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class codeUpdateViewModel : CodingLoggerUpdateViewModel
    {
        public override bool AppFieldEnabled => true;
        private readonly CodeReportDeskViewModel _codeReportDeskViewModel;


        public override ICommand SaveCommand => throw new NotImplementedException();
        public override ICommand UpdateLogCommand { get; }


        public codeUpdateViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService, 
            CodeReportDeskViewModel codeReportDeskViewModel, CodingDataDTO codingDataDTO)
            : base(navigationService, authService, dataService)
        {
            _codeReportDeskViewModel = codeReportDeskViewModel;
            ApplicationName = Constants.VisualStudio;

            UpdateLogCommand = new UpdateCommand(_navigationService, this, _codeReportDeskViewModel, _authService, _dataService, codingDataDTO, Context);
        }


        public override async Task UpdateLogCountAsync()
        {
            var count = await _dataService.LogCountAsync(Category);
            LogCount = $"{_codeReportDeskViewModel.Logs.Count} Logs";
        }
    }
}
