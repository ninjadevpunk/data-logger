using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    internal class qtREPORTViewModel : REPORTViewModel
    {
        public override ICommand SingleExport { get; } = null!;

        public qtREPORTViewModel(IDataService dataService, QtReportDeskViewModel qtReportDeskViewModel, QtDataDTO qtDataDTO)
            : base(dataService, qtReportDeskViewModel, qtDataDTO)
        {
            // TODO: SingleExport
        }

    }
}
