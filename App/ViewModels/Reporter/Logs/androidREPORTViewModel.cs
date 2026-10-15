using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    internal class androidREPORTViewModel : REPORTViewModel
    {
        public override ICommand SingleExport { get; } = null!;
        public override ICommand OpenUpdaterCommand { get; }

        public androidREPORTViewModel(NavigationService navigationService, IDataService dataService, ASReportDeskViewModel androidReportDeskViewModel, 
            AndroidDataDTO androidDataDTO)
            : base(dataService, androidReportDeskViewModel, androidDataDTO)
        {
            // TODO: SingleExport
            OpenUpdaterCommand = new OpenUpdaterCommand(navigationService, androidReportDeskViewModel, dataService, androidDataDTO);
        }


    }
}
