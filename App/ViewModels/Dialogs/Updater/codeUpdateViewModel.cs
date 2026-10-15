using Core.DTOs.Data;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Updater
{
    /// <summary>
    /// TODO
    /// </summary>
    internal sealed class codeUpdateViewModel : CodingLoggerUpdateViewModel
    {
        public override bool AppFieldEnabled => true;


        public override ICommand SaveCommand { get; } = null!;
        public override ICommand UpdateCommand { get; }


        public codeUpdateViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService,
            CodeReportDeskViewModel codeReportDeskViewModel, CodingDataDTO codingDataDTO)
            : base(navigationService, authService, dataService)
        {
            Extensions.InitialiseCodingUpdateViewModel(_navigationService, _dataService, this, codingDataDTO);
            ApplicationName = codingDataDTO.ApplicationName;

            // TODO: SaveCommand
            UpdateCommand = new UpdateLogCommand(navigationService, dataService, codeReportDeskViewModel, this, codingDataDTO);
        }


        public override async Task UpdateLogCountAsync()
        {
            var count = await _dataService.LogCountAsync(Category);
            LogCount = $"{count} Logs";
        }
    }
}
