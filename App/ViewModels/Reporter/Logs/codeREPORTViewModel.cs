using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    internal class codeREPORTViewModel : REPORTViewModel
    {
        public override ICommand SingleExport { get; } = null!;

        public codeREPORTViewModel(IDataService dataService, CodeReportDeskViewModel codeReportDeskViewModel, CodingDataDTO codingDataDTO)
            : base(dataService, codeReportDeskViewModel, codingDataDTO)
        {
            // TODO: SingleExport
        }


    }
}
